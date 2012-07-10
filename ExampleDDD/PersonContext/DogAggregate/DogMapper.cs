using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoHoney;

namespace RacingContext.DogAggregate
{
    class DogMapper : MongoMapper
    {
        public void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<Dog>(map =>
                                                   {
                                                       map.MapProperty(_ => _.Name);
                                                   });
        }
    }
}
