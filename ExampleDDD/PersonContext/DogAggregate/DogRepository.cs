using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoHoney;

namespace RacingContext.DogAggregate
{
    class DogRepository
    {
        private readonly Repository repository;
        private readonly MongoCollection<Dog> dogCollection;

        public DogRepository(Repository repository, MongoCollection<Dog> dogCollection)
        {
            this.repository = repository;
            this.dogCollection = dogCollection;
        }

        public void CreateDogAggregate(Dog dog)
        {
            dogCollection.Insert(dog);
        }

        public Dog GetById(Guid id)
        {
            return repository.GetAggregateById<Dog>(id);
        }
    }
}
