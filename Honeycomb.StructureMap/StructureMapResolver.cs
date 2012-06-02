namespace Honeycomb.StructureMap
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Events;
    using global::StructureMap;

    public class StructureMapResolver : EventConsumerResolver, CommandHandlerResolver
    {
        public IEnumerable<ConsumesEvent<TEvent>> GetConsumers<TEvent>(TEvent @event) where TEvent : Event
        {
            return ObjectFactory.GetAllInstances<ConsumesEvent<TEvent>>();
        }

        public HandlesCommand<TCommand> GetHandler<TCommand>(TCommand command) where TCommand : Command
        {
            return ObjectFactory.GetInstance<HandlesCommand<TCommand>>();
        }
    }
}