using Honeycomb.Aggregate;

namespace TestHoneycomb.TestEvents
{
    using System;
    using Honeycomb.Events;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TestInProcessEventTransport
    {
        public class DummyEvent : Event {
            public AggregateRoot Aggregate
            {
                get { throw new NotImplementedException(); }
            }
        }

        [Test]
        public void WhenSendingAnEventThenItShouldForwardToDistributor()
        {
            var subject = new InProcessEventTransport();
            var distributor = Substitute.For<EventDistributor>(null, null, new EventTransport[0]);
            subject.RegisterDistributor(distributor);

            var uniqueEvent = new UniqueEvent<DummyEvent>(Guid.NewGuid(), new DummyEvent());

            subject.Send(uniqueEvent);

            distributor.Received().Receive(uniqueEvent);
        }
    }
}
