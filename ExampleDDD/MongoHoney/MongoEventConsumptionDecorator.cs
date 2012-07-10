using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DDD.Persistence;
using Honeycomb;
using Honeycomb.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MongoHoney
{
    class MongoEventConsumptionDecorator<TConsumer, TEvent> : EventConsumptionDecorator<TConsumer,TEvent> where TConsumer : ConsumesEvent<TEvent> where TEvent : Event
    {
        private readonly MongoEventStore eventStore;
        private readonly Clock clock;

        public MongoEventConsumptionDecorator(MongoEventStore eventStore, Clock clock)
        {
            this.eventStore = eventStore;
            this.clock = clock;
        }

        public void BeforeConsumption(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer, CancelEventArgs eventArgs)
        {
            var consumptionRecord = getConsumptionRecord(uniqueEvent, consumer);

            consumptionRecord.ConsumedTime = clock();
        }

        public void AfterConsumption(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer)
        {
            var consumptionRecord = getConsumptionRecord(uniqueEvent, consumer);

            consumptionRecord.CompletedTime = clock();
        }

        private UniqueEvent.ConsumptionRecord getConsumptionRecord(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer)
        {
            var consumptionRecord = uniqueEvent.ConsumptionRecords.Single(_ => _.Consumer == consumer);
            return consumptionRecord;
        }

        public void AfterFailedConsumption(UniqueEvent<TEvent> uniqueEvent, ConsumesEvent<TEvent> consumer, UnhandledExceptionEventArgs eventArgs)
        {
            var consumptionRecord = getConsumptionRecord(uniqueEvent, consumer);

            consumptionRecord.CompletedTime = clock();
            consumptionRecord.ExceptionEventArgs = eventArgs;
        }
    }
}
