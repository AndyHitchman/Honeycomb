using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD;
using DDD.Aggregate;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoHoney
{
    public class MongoAggregateRootReference<AggregateRootType> : AggregateReference<AggregateRootType>, IBsonSerializer where AggregateRootType : AggregateRoot
    {
        private AggregateRootType aggregateRoot;
        private readonly Repository repository;
        private Guid? identity;

        public MongoAggregateRootReference(Repository repository)
        {
            this.repository = repository;
        }

        public MongoAggregateRootReference(AggregateRootType aggregateRoot)
        {
            this.aggregateRoot = aggregateRoot;
        }

        public MongoAggregateRootReference(Repository repository, Guid identity)
        {
            this.repository = repository;
            this.identity = identity;
        }

        public AggregateRootType GetAggregateRoot()
        {
            if (aggregateRoot == null && identity.HasValue)
            {
                aggregateRoot = repository.GetAggregateById<AggregateRootType>(identity.Value);
            }

            return aggregateRoot;
        }

        public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            var identity = (Guid)GuidSerializer.Instance.Deserialize(bsonReader, typeof(Guid), options);
            return new MongoAggregateRootReference<AggregateRootType>(repository, identity);
        }

        public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            var identity = (Guid)GuidSerializer.Instance.Deserialize(bsonReader, typeof(Guid), options);
            return new MongoAggregateRootReference<AggregateRootType>(repository, identity);
        }

        public IBsonSerializationOptions GetDefaultSerializationOptions()
        {
            return null;
        }

        public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            var root = value as MongoAggregateRootReference<AggregateRootType>;

            var identity = root.GetAggregateRoot().Identity;
            GuidSerializer.Instance.Serialize(bsonWriter, nominalType, identity, options);    
        }
    }
}
