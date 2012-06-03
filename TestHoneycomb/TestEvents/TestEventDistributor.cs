namespace TestHoneycomb.TestEvents
{
    using System;
    using Honeycomb.Events;
    using Microsoft.Practices.ServiceLocation;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TestEventDistributor
    {
        public class DummyEvent : Event {}

        [Test]
        public void WhenCreatingTheDistributorThenItShouldRegisterItselfWithEveryTransport()
        {
            var transport1 = Substitute.For<EventTransport>();
            var transport2 = Substitute.For<EventTransport>();

            var subject = new EventDistributor(null, null, new[] {transport1, transport2});

            transport1.Received().RegisterDistributor(subject);
            transport2.Received().RegisterDistributor(subject);
        }

        [Test]
        public void WhenReceivingAnEventThenItWillCheckTheEventStoreToEnsureItsNotAlreadyBeenConsumed()
        {
            var eventStore = Substitute.For<EventStore>();
            var subject = new EventDistributor(null, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());

            eventStore.IsEventAlreadyConsumed(@event).Returns(true);
            subject.Receive(@event);

            eventStore.Received().IsEventAlreadyConsumed(@event);
        }

        [Test]
        public void WhenReceivingAnEventThatIsAlreadyConsumedThenItShouldBeDropped()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            
            eventStore.IsEventAlreadyConsumed(@event).Returns(true);
            subject.Receive(@event);

            serviceLocator.DidNotReceiveWithAnyArgs().GetAllInstances(null);
        }

        [Test]
        public void WhenReceivingAnEventThenItWillGetAllConsumersOfTheEvent()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());

            eventStore.IsEventAlreadyConsumed(@event).Returns(false);
            serviceLocator.GetAllInstances(typeof(ConsumesEvent<>).MakeGenericType(typeof(DummyEvent))).Returns(new ConsumesEvent<DummyEvent>[0]);
            subject.Receive(@event);

            serviceLocator.Received().GetAllInstances(typeof(ConsumesEvent<>).MakeGenericType(typeof(DummyEvent)));
        }

        [Test]
        public void WhenReceivingAnEventThenItWillCallEachConsumerOfTheEvent()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer1 = Substitute.For<ConsumesEvent<DummyEvent>>();
            var consumer2 = Substitute.For<ConsumesEvent<DummyEvent>>();

            eventStore.IsEventAlreadyConsumed(@event).Returns(false);
            serviceLocator.GetAllInstances(typeof(ConsumesEvent<>).MakeGenericType(typeof(DummyEvent))).Returns(new[] {consumer1, consumer2});
            subject.Receive(@event);

            consumer1.Received().Consume(@event.Event);
            consumer2.Received().Consume(@event.Event);
        }
    }
}