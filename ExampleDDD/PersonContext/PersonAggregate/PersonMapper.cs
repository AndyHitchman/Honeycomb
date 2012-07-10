using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoHoney;
using RacingContext.DogAggregate;
using RacingContext.PersonAggregate.Entities;

namespace RacingContext.PersonAggregate
{
    class PersonMapper : MongoMapper
    {
        private readonly Repository repository;

        public PersonMapper(Repository repository)
        {
            this.repository = repository;
        }

        public void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Person>(map =>
                                                      {                                                          
                                                          map.MapProperty(_ => _.Dog).SetSerializer(new MongoAggregateRootReference<Dog>(repository));
                                                          map.MapProperty(_ => _.Registration);
                                                      });
            BsonClassMap.RegisterClassMap<Registration>(map =>
                                                            {
                                                                map.MapProperty(_ => _.RegistrationPeriodInDays);
                                                            });
        }
    }
}
