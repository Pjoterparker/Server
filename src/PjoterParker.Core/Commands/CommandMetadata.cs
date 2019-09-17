using System;

namespace PjoterParker.Core.Commands
{
    public class CommandMetadata
    {
        public Guid CommandId { get; set; }

        public Guid CorrelationId { get; set; }

        private DateTimeOffset Timestamp { get; }
    }
}
