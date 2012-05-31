namespace Honeycomb.Events
{
    using System;

    public class UniqueEvent<TEvent> where TEvent : Event
    {
        public UniqueEvent(Guid identity, TEvent @event)
        {
            Identity = identity;
            Event = @event;
        }

        /// <summary>
        /// The unique identity of the event
        /// </summary>
        public Guid Identity { get; private set; }

        /// <summary>
        /// The contained event 
        /// </summary>
        public TEvent Event { get; private set; }
    }
}