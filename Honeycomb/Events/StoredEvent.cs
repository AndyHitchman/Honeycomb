namespace Honeycomb.Events
{
    using System;

    public class StoredEvent
    {
        public UniqueEvent Event { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
    }
}