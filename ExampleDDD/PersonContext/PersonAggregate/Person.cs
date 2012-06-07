using System.Collections.Generic;
using DDD;
using PersonContext.DogAggregate;
using PersonContext.DogAggregate.Entities;
using PersonContext.PersonAggregate.Entities;

namespace PersonContext.PersonAggregate
{
    internal class Person : AggregateRoot
    {
        public Registration Registration { get; set; }

        public Dog Dog { get; set; }
    }
}
