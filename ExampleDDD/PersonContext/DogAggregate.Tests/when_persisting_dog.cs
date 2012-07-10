using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoHoney;
using NUnit.Framework;

namespace RacingContext.DogAggregate.Tests
{
    [TestFixture]
    class when_persisting_dog : MongoTest
    {
        [Test]
        public void when_creating_a_dog()
        {
            // TODO: Use a registry to do this.
            new DogMapper().RegisterClassMaps();            
            new AggregateRootMapper().RegisterClassMaps();

            var dog = new Dog()
                          {
                              Name = "Dog 1"
                          };

          
            var dogRepository = new DogRepository(new Repository(database),
                                                  database.GetCollection<Dog>(typeof (Dog).Name));

            dogRepository.CreateDogAggregate(dog);

            var dogQ = dogRepository.GetById(dog.Identity);
            Assert.AreNotEqual(dogQ, dog); // Just to make sure we pulled it from the db
            Assert.AreEqual(dogQ.Name, dog.Name);
        }
    }
}
