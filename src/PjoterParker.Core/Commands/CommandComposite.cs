using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Core.Commands
{
    public class CommandComposite
    {
        public CommandComposite(ICommand command, CommandMetadata metadata)
        {
            Command = command;
            Metadata = metadata;
        }

        public ICommand Command { get; set; }
        public CommandMetadata Metadata { get; set; }
    }
}
