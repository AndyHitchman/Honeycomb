namespace Honeycomb.Commands
{
    public interface HandlesCommand<TCommand> where TCommand : Command
    {
        CommandApplication TryApply(TCommand command);
    }
}