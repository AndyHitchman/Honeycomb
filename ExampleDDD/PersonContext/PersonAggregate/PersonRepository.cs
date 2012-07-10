using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using MongoDB.Driver;
using MongoHoney;

namespace RacingContext.PersonAggregate
{
    class PersonRepository
    {
        private readonly Repository repository;
        private readonly MongoCollection<Person> personCollection;

        public PersonRepository(Repository repository, MongoCollection<Person> personCollection)
        {
            this.repository = repository;
            this.personCollection = personCollection;
        }

        public void CreatePersonAggregate(Person person)
        {
            personCollection.Insert(person);
        }

        public Person GetById(Guid id)
        {
            return repository.GetAggregateById<Person>(id);
        }
    }
}
