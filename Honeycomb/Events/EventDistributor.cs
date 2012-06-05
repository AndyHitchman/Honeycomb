namespace Honeycomb.Events
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Practices.ServiceLocation;

    public class EventDistributor
    {
        private readonly IServiceLocator serviceLocator;
        private readonly EventStore eventStore;
        private readonly IEnumerable<EventTransport> eventTransports;

        public EventDistributor(
            IServiceLocator serviceLocator, EventStore eventStore, 
            IEnumerable<EventTransport> eventTransports)
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
                var decorator = serviceLocator.GetInstance<EventConsumptionDecorator<ConsumesEvent<TEvent>, TEvent>>();
                var cancelEventArgs = new CancelEventArgs();

                decorator.BeforeConsumption(@event, consumer, cancelEventArgs);
                if (cancelEventArgs.Cancel) continue;

                try
                {
                    try
                    {
                        consumer.Consume(@event.Event);
                    }
                    catch (Exception e)
                    {
                        decorator.AfterFailedConsumption(@event, consumer, new UnhandledExceptionEventArgs(e, false));
                        throw;
                    }
                    decorator.AfterConsumption(@event, consumer);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
        }

        /// <summary>
        /// Raise an event. Will be propagated over all registered transports
        /// </summary>
        /// <typeparam name = "TEvent"></typeparam>
        /// <param name = "event"></param>
        /// <returns></returns>
        public virtual void Raise<TEvent>(TEvent @event) where TEvent : Event
        {
            var uniqueEvent = new UniqueEvent<TEvent>(Guid.NewGuid(), @event);

            foreach (var transport in eventTransports)
            {
                transport.Send(uniqueEvent);
            }
        }

        public virtual IEnumerable<ConsumesEvent<TEvent>> GetConsumers<TEvent>(TEvent @event) where TEvent : Event
        {
            return (IEnumerable<ConsumesEvent<TEvent>>)serviceLocator.GetAllInstances<ConsumesEvent<TEvent>>();
        }
    }
}