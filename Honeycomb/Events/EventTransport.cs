namespace Honeycomb.Events
{
    /// <summary>
    /// The transport for events
    /// </summary>
    public interface EventTransport
    {
        ///<summary>
        /// Propagate the event over the transport.
        ///</summary>
        ///<param name="event"></param>
        ///<typeparam name="TEvent"></typeparam>
        void Propagate<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;

        /// <summary>
        /// An received event should be consumed within the bounded context. 
        /// </summary>
        /// <remarks>
        /// Double-handling of events must be prevented.
        /// <para/>
        /// An internally raised event should also be consumed. The implementation should ensure transactional isolation.
        /// </remarks>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        void Consume<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;
    }
}