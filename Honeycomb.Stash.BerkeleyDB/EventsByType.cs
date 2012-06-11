namespace Honeycomb.Stash.BerkeleyDB
{
    using System.Collections.Generic;
    using Events;
    using global::Stash;

    public class EventsByType : IIndex<StoredEvent,string>
    {
        public IEnumerable<string> Yield(StoredEvent graph)
        {
            yield return graph.Event.EventType.ToString();
        }
    }
}