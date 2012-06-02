namespace Honeycomb.Events
{
    using System.Collections.Generic;

    public interface EventConsumerResolver
    {
        IEnumerable<ConsumesEvent<TEvent>> GetConsumers<TEvent>(TEvent @event) where TEvent : Event;
    }
}