namespace PjoterParker.Core.Commands
{
    public class CommandComposite
    {
        public CommandComposite(ICommand command)
        {
            Command = command;
        }

        public ICommand Command { get; set; }

        public CommandMetadata Metadata { get; set; } = new CommandMetadata();
    }
}
