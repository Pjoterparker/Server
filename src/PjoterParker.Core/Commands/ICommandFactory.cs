namespace PjoterParker.Core.Commands
{
    public interface ICommandFactory
    {
        CommandComposite Make(ICommand command);
    }
}
