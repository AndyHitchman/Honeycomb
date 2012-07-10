using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;

namespace RacingContext.DogAggregate
{
    internal class Dog : AggregateRoot
    {
        public string Name { get; set; }
    }
}
