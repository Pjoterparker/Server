using System.Collections.Generic;
using PjoterParker.Core.Commands;

namespace PjoterParker.Core.Events
{
    public interface IEventFactory
    {
        void Make(CommandComposite command, IEnumerable<EventComposite> @event);
    }
}
