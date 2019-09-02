using System;

namespace PjoterParker.Core.Commands
{
    public class EventMetadata
    {
        public Guid EventId { get; set; }

        public Guid CommandId { get; set; }

        public Guid CorrelationId { get; set; }

        private DateTimeOffset Timestamp { get; }
    }
}