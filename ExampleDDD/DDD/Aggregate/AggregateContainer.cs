using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Events;

namespace DDD
{
    public class AggregateContainer
    {
        private readonly EventStore eventStore;
        private readonly EventDistributor eventDistributor;
        private IDictionary<Guid, AggregateRoot> aggregateRoots = new Dictionary<Guid, AggregateRoot>(); 


        public AggregateContainer(EventStore eventStore, EventDistributor eventDistributor)
        {
            this.eventStore = eventStore;
            this.eventDistributor = eventDistributor;
        }

        public void RegisterAggregate(AggregateRoot aggregateRoot)
        {
            aggregateRoots[aggregateRoot.Id] = aggregateRoot;
        }

        public AggregateRoot MaterialiseAggregate(Guid aggregateId)
        {
            // Note: The first event in the sequence should actually create the aggregate, so it should be registered by
            // the time we have consumed all events
            var eventsForAggregate = eventStore.LoadEventsForAggregate(aggregateId);

            foreach (var @event in eventsForAggregate)
            {
                eventDistributor.Receive(@event);
            }

            return aggregateRoots[aggregateId];
        }

    }
}
