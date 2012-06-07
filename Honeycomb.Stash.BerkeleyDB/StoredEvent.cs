namespace Honeycomb.Stash.BerkeleyDB
{
    using System;
    using Events;

    public class StoredEvent
    {
        public UniqueEvent Event { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
    }
}