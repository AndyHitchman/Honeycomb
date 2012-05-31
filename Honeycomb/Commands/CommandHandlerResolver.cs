namespace Honeycomb.Commands
{
    public interface CommandHandlerResolver
    {
        HandlesCommand<TCommand> GetResolver<TCommand>(TCommand command) where TCommand : Command;
    }
}