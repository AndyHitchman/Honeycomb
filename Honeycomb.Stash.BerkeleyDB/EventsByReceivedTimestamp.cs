namespace Honeycomb.Stash.BerkeleyDB
{
    using System;
    using System.Collections.Generic;
    using Events;
    using global::Stash;

    public class EventsByReceivedTimestamp : IIndex<StoredEvent,DateTime>
    {
        public IEnumerable<DateTime> Yield(StoredEvent graph)
        {
            yield return graph.ReceivedTimestamp;
        }
    }
}