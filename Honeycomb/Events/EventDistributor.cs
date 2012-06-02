namespace Honeycomb.Events
{
    using System;
    using System.Collections.Generic;

    public class EventDistributor
    {
        private readonly EventStore eventStore;
        private readonly EventConsumerResolver eventConsumerResolver;
        private readonly IEnumerable<EventTransport> eventTransports;

        public EventDistributor(EventStore eventStore, EventConsumerResolver eventConsumerResolver, IEnumerable<EventTransport> eventTransports)
        {
            this.eventStore = eventStore;
            this.eventConsumerResolver = eventConsumerResolver;
            this.eventTransports = eventTransports;

            foreach (var transport in eventTransports)
            {
                transport.RegisterDistributor(this);
            }
        }

        public virtual void Receive<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event
        {
            if (eventStore.IsEventAlreadyConsumed(@event)) return;

            var consumers = eventConsumerResolver.GetConsumers(@event.Event);

            foreach (var consumer in consumers)
            {
                consumer.Consume(@event.Event);
            }
        }

        /// <summary>
        /// Raise an event. Will be propagated over all registered transports
        /// </summary>
        /// <typeparam name = "TEvent"></typeparam>
        /// <param name = "event"></param>
        /// <returns></returns>
        public void Raise<TEvent>(TEvent @event) where TEvent : Event
        {
            var uniqueEvent = new UniqueEvent<TEvent>(Guid.NewGuid(), @event);

            foreach (var transport in eventTransports)
            {
                transport.Send(uniqueEvent);
            }
        }

    }
}