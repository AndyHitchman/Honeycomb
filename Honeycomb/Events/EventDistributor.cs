namespace Honeycomb.Events
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;

    public class EventDistributor
    {
        private readonly IServiceLocator serviceLocator;
        private readonly EventStore eventStore;
        private readonly IEnumerable<EventTransport> eventTransports;

        public EventDistributor(IServiceLocator serviceLocator, EventStore eventStore, IEnumerable<EventTransport> eventTransports)
        {
            this.serviceLocator = serviceLocator;
            this.eventStore = eventStore;
            this.eventTransports = eventTransports;

            foreach (var transport in eventTransports)
            {
                transport.RegisterDistributor(this);
            }
        }

        public virtual void Receive<TEvent>(UniqueEvent<TEvent> @event) where TEvent : Event
        {
            if (eventStore.IsEventAlreadyConsumed(@event)) return;

            var consumers = GetConsumers(@event.Event);

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

        public virtual IEnumerable<ConsumesEvent<TEvent>> GetConsumers<TEvent>(TEvent @event) where TEvent : Event
        {
            return (IEnumerable<ConsumesEvent<TEvent>>)serviceLocator.GetAllInstances(typeof(ConsumesEvent<>).MakeGenericType(typeof(TEvent)));
        }
    }
}