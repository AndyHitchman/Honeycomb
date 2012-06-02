namespace TestHoneycomb.TestEvents
{
    using System;
    using Honeycomb.Events;
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
            var eventConsumerResolver = Substitute.For<EventConsumerResolver>();
            var subject = new EventDistributor(eventStore, eventConsumerResolver, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());

            subject.Receive(@event);

            eventStore.Received().IsEventAlreadyConsumed(@event);
        }

        [Test]
        public void WhenReceivingAnEventThatIsAlreadyConsumesThenItShouldBeDropped()
        {
            var eventStore = Substitute.For<EventStore>();
            var subject = new EventDistributor(eventStore, null, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            
            eventStore.IsEventAlreadyConsumed(@event).Returns(true);
            subject.Receive(@event);


        }

        [Test]
        public void WhenReceivingAnEventThenItWillGetAllConsumersOfTheEvent()
        {
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerResolver = Substitute.For<EventConsumerResolver>();
            var subject = new EventDistributor(eventStore, eventConsumerResolver, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());

            eventStore.IsEventAlreadyConsumed(@event).Returns(false);
            subject.Receive(@event);

            eventConsumerResolver.Received().GetConsumers(@event.Event);
        }

        [Test]
        public void WhenReceivingAnEventThenItWillCallEachConsumerOfTheEvent()
        {
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerResolver = Substitute.For<EventConsumerResolver>();
            var subject = new EventDistributor(eventStore, eventConsumerResolver, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer1 = Substitute.For<ConsumesEvent<DummyEvent>>();
            var consumer2 = Substitute.For<ConsumesEvent<DummyEvent>>();

            eventStore.IsEventAlreadyConsumed(@event).Returns(false);
            eventConsumerResolver.GetConsumers(@event.Event).Returns(new[] {consumer1, consumer2});
            subject.Receive(@event);

            eventConsumerResolver.Received().GetConsumers(@event.Event);

            consumer1.Received().Consume(@event.Event);
            consumer2.Received().Consume(@event.Event);
        }
    }
}