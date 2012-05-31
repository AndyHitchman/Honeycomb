using System;
using System.Collections.Generic;
using System.Linq;

namespace Honeycomb.Events
{
    public class TestEventTransport : EventTransport
    {
        /// <summary>
        ///   Each thread has its own callback. Used primarily in testing.
        /// </summary>
        private readonly List<Delegate> eventHandlers = new List<Delegate>();


        public void Propagate<TEvent>(UniqueEvent<TEvent> domainEvent) where TEvent : Event
        {
            if (eventHandlers == null) return;

            foreach (var handler in eventHandlers.OfType<Action<UniqueEvent<TEvent>>>())
                handler(domainEvent);
        }

        public void Consume<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Registers a callback to receive the unique event.
        /// </summary>
        /// <param name = "callback"></param>
        public TestEventTransport Register<TEvent>(Action<UniqueEvent<TEvent>, DateTime> callback) where TEvent : Event
        {
            eventHandlers.Add(callback);
            return this;
        }

        /// <summary>
        ///   Registers a callback to receive the unique event.
        /// </summary>
        /// <param name = "callback"></param>
        public TestEventTransport Register<TEvent>(Action<UniqueEvent<TEvent>> callback) where TEvent : Event
        {
            eventHandlers.Add(callback);
            return this;
        }
    }
}