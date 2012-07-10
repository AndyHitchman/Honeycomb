using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Honeycomb.Events;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DDD.Persistence
{
    public class MongoEventStore : EventStore
    {
        private MongoDatabase database;
        private const string databaseName = "";
        private const string connectionString = "";

        public MongoEventStore()
        {
            var server = MongoServer.Create(connectionString);
            database = server.GetDatabase(databaseName);
        }

        public bool IsEventAlreadyConsumed<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event
        {
            return EventCollection<TEvent>().AsQueryable().Any(_ => _.Identity == @event.Identity && _.ConsumptionRecords.Any());
        }

        public MongoCollection<UniqueEvent<Event>> EventCollection<TEvent>() where TEvent : Event
        {
            return database.GetCollection<UniqueEvent<Event>>(typeof(UniqueEvent<Event>).Name);
        }

        public IEnumerable<UniqueEvent<Event>> LoadEventsForAggregate(Guid aggregateId)
        {
            return EventCollection<Event>().Find(new QueryDocument("AggregateId", aggregateId));
        }
    }
}
