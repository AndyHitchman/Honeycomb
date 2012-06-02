namespace Honeycomb.Events
{
    public class InProcessEventTransport : EventTransport
    {
        private EventDistributor eventDistributor;

        public void Send<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event
        {
            eventDistributor.Receive(@event);
        }

        public void RegisterDistributor(EventDistributor distributor)
        {
            eventDistributor = distributor;
        }
    }
}