using Honeycomb;
using MongoDB.Bson.Serialization;
using MongoHoney;
using NUnit.Framework;
using RacingContext.DogAggregate;
using RacingContext.PersonAggregate.Entities;

namespace RacingContext.PersonAggregate.Tests
{
    [TestFixture]
    class when_persisting_person : MongoTest
    {
        [Test]
        public void when_creating_a_person()
        {
            // TODO: Use a registry to do this.
            var repository = new Repository(database);
            var dogRepository = new DogRepository(repository, database.GetCollection<Dog>(typeof(Dog).Name));
            var personRepository = new PersonRepository(repository, database.GetCollection<Person>(typeof(Person).Name));

            new DogMapper().RegisterClassMaps();
            new PersonMapper(repository).RegisterClassMaps();
            new AggregateRootMapper().RegisterClassMaps();

            var person = new Person();
            person.Registration = new Registration(person, new IdentityGenerator().NewId());
            var dog = new Dog() { Name = "Dog 2"};
            person.Dog = new MongoAggregateRootReference<Dog>(dog);

            dogRepository.CreateDogAggregate(dog);            

            personRepository.CreatePersonAggregate(person);

            var personQ = personRepository.GetById(person.Identity);
            Assert.AreNotEqual(personQ, person); // Just to make sure we pulled it from the db
            Assert.AreNotEqual(personQ.Dog, dog);
            Assert.AreEqual(personQ.Dog.GetAggregateRoot().Identity, dog.Identity);
            Assert.AreEqual(personQ.Dog.GetAggregateRoot().Name, dog.Name);
            Assert.AreNotEqual(personQ.Registration, person.Registration);
            Assert.AreEqual(personQ.Registration.RegistrationPeriodInDays, person.Registration.RegistrationPeriodInDays);
        }
    }
}
