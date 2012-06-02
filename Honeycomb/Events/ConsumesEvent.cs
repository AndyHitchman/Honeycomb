namespace Honeycomb.Events
{
    /// <summary>
    ///   Consume an event.
    /// </summary>
    /// <typeparam name = "TEvent"></typeparam>
    public interface ConsumesEvent<in TEvent> where TEvent : Event
    {
        /// <summary>
        ///   Receive an event
        /// </summary>
        /// <param name = "event"></param>
        void Consume(TEvent @event);
    }
}