using System.Collections.Generic;
using DDD;
using DDD.Aggregate;
using RacingContext.DogAggregate;
using RacingContext.PersonAggregate.Entities;

namespace RacingContext.PersonAggregate
{
    internal class Person : AggregateRoot
    {
        public Registration Registration { get; set; }

        public AggregateReference<Dog> Dog { get; set; }
    }
}
