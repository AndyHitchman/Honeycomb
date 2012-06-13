namespace DogLifecycle
{
    using CrossContext;
    using Events;

    public class DogSelector : SelectAggregate<Dog,DogIsNamed>
    {
        public Dog Select(DogIsNamed @event)
        {
                
        }
    }
}