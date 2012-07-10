using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using MongoDB.Bson.Serialization;
using MongoHoney;

namespace RacingContext
{
    class AggregateRootMapper : MongoMapper 
    {
        public void RegisterClassMaps()
        {
            BsonClassMap.RegisterClassMap<AggregateRoot>(map =>
                                                             {
                                                                 map.MapIdProperty(_ => _.Identity);
                                                             });
        }
    }
}
