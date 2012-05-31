namespace Honeycomb.Events
{
    public interface EventDistributor
    {
        void Receive<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;
    }
}