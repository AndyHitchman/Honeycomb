using Honeycomb.Aggregate;

namespace Honeycomb.Events
{
    /// <summary>
    ///   Events represent an irrefutable factual event.
    ///   Therefore they 
    ///   (1) should not be able to interfere with the causation, i.e. they are transactionally isolated when handled.
    ///   (2) should only be raised if the transaction raising the events commits.
    /// </summary>
    public interface Event
    {
        AggregateRoot Aggregate { get; }
    }
}