namespace Honeycomb.Events
{
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;

    public class EventConsumerResolver
    {
        public virtual IEnumerable<ConsumesEvent<TEvent>> GetConsumers<TEvent>(TEvent @event) where TEvent : Event
        {
            return (IEnumerable<ConsumesEvent<TEvent>>) ServiceLocator.Current.GetAllInstances(typeof (ConsumesEvent<>).MakeGenericType(typeof (TEvent)));
        }
    }
}