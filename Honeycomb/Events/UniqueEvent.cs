namespace Honeycomb.Events
{
    using System;

    public class UniqueEvent
    {
        protected UniqueEvent(Guid identity, Event @event, DateTime raisedTimestamp)
        {
            Identity = identity;
            UntypedEvent = @event;
            RaisedTimestamp = raisedTimestamp;
            EventType = @event.GetType();
        }

        /// <summary>
        /// The unique identity of the event
        /// </summary>
        public Guid Identity { get; private set; }

        /// <summary>
        /// The event
        /// </summary>
        public Event UntypedEvent { get; private set; }

        /// <summary>
        /// Type of the event
        /// </summary>
        public Type EventType { get; private set; }

        /// <summary>
        /// The timestamp when the event was raised.
        /// </summary>
        public DateTime RaisedTimestamp { get; private set; }
    }
}