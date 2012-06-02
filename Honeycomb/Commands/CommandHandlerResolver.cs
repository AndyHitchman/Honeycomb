namespace Honeycomb.Commands
{
    using Microsoft.Practices.ServiceLocation;

    public class CommandHandlerResolver
    {
        public virtual HandlesCommand<TCommand> GetHandler<TCommand>(TCommand command) where TCommand : Command
        {
            return (HandlesCommand<TCommand>)ServiceLocator.Current.GetInstance(typeof(HandlesCommand<>).MakeGenericType(typeof(TCommand)));            
        }
    }
}