using System;

namespace PjoterParker.Core.Commands
{
    public class EventMetadata
    {
        public Guid CommandId { get; set; }

        public Guid CorrelationId { get; set; }

        public Guid EventId { get; set; }
        public string EventType { get; set; }

        private DateTimeOffset Timestamp { get; }
    }
}
