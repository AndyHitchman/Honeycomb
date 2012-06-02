namespace Honeycomb.Events
{
    using Microsoft.Practices.ServiceLocation;

    public static class EventExtensions
    {
        private static readonly EventDistributor eventDistributor;

        static EventExtensions()
        {
            eventDistributor = ServiceLocator.Current.GetInstance<EventDistributor>();
        }

        public static void Raise(this Event @event)
        {
            eventDistributor.Raise(@event);
        }
    }
}