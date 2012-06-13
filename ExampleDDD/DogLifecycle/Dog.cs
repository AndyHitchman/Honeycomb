namespace DogLifecycle
{
    using System;
    using System.Collections.Generic;
    using CrossContext;
    using Events;
    using Honeycomb.Events;

    public class Dog : Aggregate, ConsumesEvent<DogIsNamed>
    {
        public class Naming
        {
            public Naming(string name, DateTime dateNamed)
            {
                Name = name;
                DateNamed = dateNamed;
            }

            public string Name { get; private set; }
            public DateTime DateNamed { get; private set; }
        }

        /// <summary>
        /// Construct a new dog with an earbrand, typically occurring following litter registration.
        /// </summary>
        /// <param name="event"></param>
        public Dog(DogIsRegistered @event) : this()
        {
            Earbrand = @event.Earbrand;
        }

        public Dog()
        {
            previousNames = new List<Naming>();
        }

        /// <summary>
        /// The dog's earbrand, which the business enforces as a unique identifier.
        /// </summary>
        public Earbrand Earbrand { get; private set; }

        public Naming Named { get; private set; }

        public IEnumerable<Naming> PreviousNames
        {
            get { return previousNames; }
        }
        private readonly List<Naming> previousNames;

        public void Consume(DogIsNamed @event)
        {
            throw new NotImplementedException();
        }
    }
}