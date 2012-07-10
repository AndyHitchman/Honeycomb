using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoHoney
{
    public class Repository
    {
        private readonly MongoDatabase database;

        public Repository(MongoDatabase database)
        {
            this.database = database;
        }

        public AggregateType GetAggregateById<AggregateType>(Guid id) where AggregateType : Identifiable
        {
            return database.GetCollection<AggregateType>(typeof (AggregateType).Name).AsQueryable().Single(_ => _.Identity == id);
        }
    }
}
