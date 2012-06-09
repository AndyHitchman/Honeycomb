using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;

namespace RacingContext.DogAggregate.Entities
{
    internal class LifeState : Entity<Dog>
    {
        public LifeState(Dog aggregateRoot, Guid id) : base(aggregateRoot, id)
        {
        }

        public Dog Dog { get; set; }
    }
}
