namespace Honeycomb.Stash.BerkeleyDB
{
    using System;
    using System.Collections.Generic;
    using global::Stash;

    public class EventByReceivedTimestamp : IIndex<StoredEvent,DateTime>
    {
        public IEnumerable<DateTime> Yield(StoredEvent graph)
        {
            yield return graph.ReceivedTimestamp;
        }
    }
}