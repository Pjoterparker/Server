using PjoterParker.Core.Commands;
using PjoterParker.Core.Services;

namespace PjoterParker.Common.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private IGuidService _guid;

        public CommandFactory(IGuidService guid)
        {
            _guid = guid;
        }

        public CommandComposite Make(ICommand command)
        {
            var commandComposite = new CommandComposite(command);
            commandComposite.Metadata.CommandId = _guid.New();
            commandComposite.Metadata.CorrelationId = _guid.New();

            return commandComposite;
        }
    }
}