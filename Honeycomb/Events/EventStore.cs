using System;
using System.Collections.Generic;

namespace Honeycomb.Events
{
    public interface EventStore
    {
        bool IsEventAlreadyConsumed<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event;

        IEnumerable<UniqueEvent<Event>> LoadEventsForAggregate(Guid aggregateId);
    }
}