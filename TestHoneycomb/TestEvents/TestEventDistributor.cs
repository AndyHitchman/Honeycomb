namespace TestHoneycomb.TestEvents
{
    using System;
    using System.ComponentModel;
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
        public void WhenRaisingAnEventItShouldBeUniquelyIdentifiedWhenSending()
        {
            var transport1 = Substitute.For<EventTransport>();
            var transport2 = Substitute.For<EventTransport>();
            var subject = new EventDistributor(null, null, new[] { transport1, transport2 });
            var actual = new DummyEvent();

            subject.Raise(actual);

            transport1.Received().Send(Arg.Is<UniqueEvent<DummyEvent>>(_ => _.Identity != Guid.Empty));
            transport1.Received().Send(Arg.Is<UniqueEvent<DummyEvent>>(_ => _.Event == actual));
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
            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new ConsumesEvent<DummyEvent>[0]);

            subject.Receive(@event);

            serviceLocator.Received().GetAllInstances<ConsumesEvent<DummyEvent>>();
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

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] {consumer1, consumer2});

            subject.Receive(@event);

            consumer1.Received().Consume(@event.Event);
            consumer2.Received().Consume(@event.Event);
        }

        [Test]
        public void WhenConsumingAnEventThenItWillCallTheBeforeDecoratorImplementation()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerDecorator = Substitute.For<EventConsumptionDecorator<ConsumesEvent<DummyEvent>,DummyEvent>>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer = Substitute.For<ConsumesEvent<DummyEvent>>();

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] { consumer });
            serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<DummyEvent>,DummyEvent>>().Returns(eventConsumerDecorator);

            subject.Receive(@event);

            eventConsumerDecorator.Received().BeforeConsumption(@event, consumer, Arg.Any<CancelEventArgs>());
        }

        [Test]
        public void WhenConsumingAnEventAndTheDecoratorCancelsThenItWillNotCallTheConsumer()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerDecorator =
                Substitute.For<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer = Substitute.For<ConsumesEvent<DummyEvent>>();

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] {consumer});
            serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>().Returns(eventConsumerDecorator);

            eventConsumerDecorator.BeforeConsumption(@event, consumer, Arg.Do<CancelEventArgs>(args => { args.Cancel = true; }));

            subject.Receive(@event);

            consumer.DidNotReceiveWithAnyArgs().Consume(null);
        }

        [Test]
        public void WhenConsumingAnEventThenItWillCallTheAfterDecoratorImplementation()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerDecorator = Substitute.For<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer = Substitute.For<ConsumesEvent<DummyEvent>>();

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] { consumer });
            serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>().Returns(eventConsumerDecorator);

            subject.Receive(@event);

            eventConsumerDecorator.Received().AfterConsumption(@event, consumer);
        }

        [Test]
        public void WhenConsumingAnEventAndTheDecoratorCancelsThenItWillNotCallTheAfterOrAfterFailedDecorator()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerDecorator =
                Substitute.For<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer = Substitute.For<ConsumesEvent<DummyEvent>>();

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] { consumer });
            serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>().Returns(eventConsumerDecorator);

            eventConsumerDecorator.BeforeConsumption(@event, consumer, Arg.Do<CancelEventArgs>(args => { args.Cancel = true; }));

            subject.Receive(@event);

            eventConsumerDecorator.DidNotReceiveWithAnyArgs().AfterConsumption(null, null);
            eventConsumerDecorator.DidNotReceiveWithAnyArgs().AfterFailedConsumption(null, null, null);
        }

        [Test]
        public void WhenConsumingAndAnExceptionIsThrownThenItWillCallTheAfterFailureDecoratorImplementationButNotTheAfterDecoratorImplementation()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var eventStore = Substitute.For<EventStore>();
            var eventConsumerDecorator = Substitute.For<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>();
            var subject = new EventDistributor(serviceLocator, eventStore, new EventTransport[0]);
            var identity = Guid.NewGuid();
            var @event = new UniqueEvent<DummyEvent>(identity, new DummyEvent());
            var consumer = Substitute.For<ConsumesEvent<DummyEvent>>();

            serviceLocator.GetAllInstances<ConsumesEvent<DummyEvent>>().Returns(new[] { consumer });
            serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<DummyEvent>, DummyEvent>>().Returns(eventConsumerDecorator);
            var expectedException = new Exception();
            consumer.When(_ => _.Consume(@event.Event)).Do((_ => { throw expectedException; }));

            subject.Receive(@event);

            eventConsumerDecorator
                .Received()
                .AfterFailedConsumption(@event, consumer, Arg.Is<UnhandledExceptionEventArgs>(args => args.ExceptionObject == expectedException));
            eventConsumerDecorator.DidNotReceiveWithAnyArgs().AfterConsumption(null, null);
        }
    }
}