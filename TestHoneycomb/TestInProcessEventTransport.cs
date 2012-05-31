using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHoneycomb
{
    using Honeycomb.Events;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TestInProcessEventTransport
    {
        private class DummyEvent : Event {}

        [Test]
        public void SendingAnEventShouldForwardToDistributor()
        {
            var subject = new InProcessEventTransport();
            var distributor = Substitute.For<EventDistributor>();
            subject.SubscribeReceiver(distributor);

            var uniqueEvent = new UniqueEvent<DummyEvent>(Guid.NewGuid(), new DummyEvent());

            subject.Send(uniqueEvent);

            distributor.Received().Receive(uniqueEvent);
        }
    }
}
