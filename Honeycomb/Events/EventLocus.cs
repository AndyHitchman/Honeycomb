using System.Collections.Generic;

namespace Honeycomb.Events
{
    using System;

    /// <summary>
    ///   The locus for managing events within the bounded context.
    /// </summary>
    public static class EventLocus
    {
        private static readonly List<EventTransport> transports = new List<EventTransport>();

        ///<summary>
        /// The transports registered for handling events.
        ///</summary>
        public static IEnumerable<EventTransport> Transports
        {
            get { return transports.AsReadOnly(); }
        }

        ///<summary>
        /// Add a transports for this context.
        ///</summary>
        ///<param name="eventTransport"></param>
        public static void RegisterTransport(EventTransport eventTransport)
        {
            transports.Add(eventTransport);
        }

        /// <summary>
        /// Raise an event. Will be propagated over all registered transports
        /// </summary>
        /// <typeparam name = "TEvent"></typeparam>
        /// <param name = "event"></param>
        /// <returns></returns>
        public static void Raise<TEvent>(this TEvent @event) where TEvent : Event
        {
            var uniqueEvent = new UniqueEvent<TEvent>(Guid.NewGuid(), @event);

            foreach (var transport in Transports)
            {
                transport.Propagate(uniqueEvent);
            }
        }
    }
}