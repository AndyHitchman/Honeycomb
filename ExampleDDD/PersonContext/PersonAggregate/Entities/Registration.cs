using DDD;

namespace PersonContext.PersonAggregate.Entities
{
    internal class Registration : Entity<Person>
    {
        public Registration(Person aggregateRoot, int id) : base(aggregateRoot, id)
        {
        }
    }
}
