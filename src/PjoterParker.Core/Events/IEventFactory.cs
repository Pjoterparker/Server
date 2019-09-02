using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Commands;

namespace PjoterParker.Core.Events
{
    public interface IEventFactory
    {
        void Make(CommandComposite command, IEvent @event);
    }
}
