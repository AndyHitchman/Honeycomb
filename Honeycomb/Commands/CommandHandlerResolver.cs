namespace Honeycomb.Commands
{
    public interface CommandHandlerResolver
    {
        HandlesCommand<TCommand> GetHandler<TCommand>(TCommand command) where TCommand : Command;
    }
}