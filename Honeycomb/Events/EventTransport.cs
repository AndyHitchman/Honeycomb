namespace Honeycomb.Events
{
    using System.Collections.Generic;

    /// <summary>
    /// The transport for events.
    /// </summary>
    public interface EventTransport
    {
        ///<summary>
        /// Propagate the event over the transport.
        ///</summary>
        ///<param name="event"></param>
        ///<typeparam name="TEvent"></typeparam>
        void Send<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;

        /// <summary>
        /// Subscribes the (one) receiver to distribute events to be consumed within the bounded context. 
        /// </summary>
        void RegisterDistributor(EventDistributor distributor);
    }
}