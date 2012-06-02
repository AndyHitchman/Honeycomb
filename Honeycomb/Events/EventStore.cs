namespace Honeycomb.Events
{
    public interface EventStore
    {
        bool IsEventAlreadyConsumed<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;
    }
}