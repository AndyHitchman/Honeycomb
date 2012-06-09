using System.Collections.Generic;
using DDD;
using RacingContext.DogAggregate;
using RacingContext.PersonAggregate.Entities;

namespace RacingContext.PersonAggregate
{
    internal class Person : AggregateRoot
    {
        public Registration Registration { get; set; }

        public Dog Dog { get; set; }
    }
}
