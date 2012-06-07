using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using PersonContext.PersonAggregate;
using PersonContext.PersonAggregate.Entities;

namespace PersonContext.DogAggregate.Entities
{
    internal class LifeState : Entity<Dog>
    {
        public LifeState(Dog aggregateRoot, int id) : base(aggregateRoot, id)
        {
        }

        public Dog Dog { get; set; }
    }
}
