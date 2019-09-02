using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Core.Commands
{
    interface ICommandFactory
    {
        CommandComposite Make(ICommand command);
    }
}
