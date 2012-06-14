namespace DogLifecycle
{
    using System;
    using CrossContext;
    using Events;
    using Honeycomb.Events;

    public class DogSelector : 
        SelectAggregate<Dog,DogIsNamed>, 
        SelectAggregate<Dog,DogIsRegistered>
    {
        private readonly EventStore eventStore;

        public DogSelector(EventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public Dog Select(DogIsNamed @event)
        {
//            return eventStore.
            throw new NotImplementedException();
        }

        public Dog Select(DogIsRegistered @event)
        {
            return new Dog(@event);
        }
    }
}