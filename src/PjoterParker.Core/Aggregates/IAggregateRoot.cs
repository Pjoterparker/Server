using System;
using System.Collections.Generic;
using PjoterParker.Core.Commands;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateRoot
    {
        IEnumerable<EventComposite> Events { get; }

        Guid Id { get; set; }

        long Version { get; set; }
    }
}
