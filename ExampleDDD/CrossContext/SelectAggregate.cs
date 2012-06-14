namespace CrossContext
{
    using Honeycomb.Events;

    public interface SelectAggregate<TAggregate,TEvent> where TAggregate : Aggregate where TEvent : Event
    {
        TAggregate Select(TEvent @event);
    }
}