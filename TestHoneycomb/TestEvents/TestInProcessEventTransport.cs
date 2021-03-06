﻿namespace TestHoneycomb.TestEvents
{
    using System;
    using Honeycomb.Events;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TestInProcessEventTransport
    {
        public class DummyEvent : Event {
        }

        [Test]
        public void WhenSendingAnEventThenItShouldForwardToDistributor()
        {
            var subject = new InProcessEventTransport();
            var distributor = Substitute.For<EventDistributor>(null, null, new EventTransport[0], null);
            subject.RegisterDistributor(distributor);

            var uniqueEvent = new UniqueEvent<DummyEvent>(Guid.NewGuid(), new DummyEvent(), DateTime.MinValue);

            subject.Send(uniqueEvent);

            distributor.Received().Receive(uniqueEvent);
        }
    }
}
