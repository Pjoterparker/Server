using System.Collections.Generic;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;
using PjoterParker.Core.Services;

namespace PjoterParker.Common.Events
{
    public class EventFactory : IEventFactory
    {
        private IGuidService _guid;

        public EventFactory(IGuidService guid)
        {
            _guid = guid;
        }

        public IEnumerable<EventComposite> Make(CommandComposite commandComposite, IEnumerable<EventComposite> @events)
        {
            foreach (var @event in @events)
            {
                @event.Metadata.EventId = _guid.New();
                @event.Metadata.CorrelationId = commandComposite.Metadata.CorrelationId;
                @event.Metadata.CommandId = commandComposite.Metadata.CommandId;
                @event.Metadata.EventType = @event.Event.GetType().AssemblyQualifiedName;
            }

            return @events;
        }
    }
}
