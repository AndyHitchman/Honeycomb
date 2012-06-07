namespace Honeycomb.Events
{
    using System;

    public class UniqueEvent<TEvent> : UniqueEvent where TEvent : Event
    {
        public UniqueEvent(Guid identity, TEvent @event, DateTime raisedTimestamp) : base(identity, @event, raisedTimestamp)
        {
        }

        /// <summary>
        /// The contained event 
        /// </summary>
        public TEvent Event { get { return (TEvent) UntypedEvent; } }
    }
}