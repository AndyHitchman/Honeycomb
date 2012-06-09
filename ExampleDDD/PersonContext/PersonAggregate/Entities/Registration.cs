using System;
using DDD;

namespace RacingContext.PersonAggregate.Entities
{
    internal class Registration : Entity<Person>
    {
        public Registration(Person aggregateRoot, Guid id) : base(aggregateRoot, id)
        {
        }
    }
}
