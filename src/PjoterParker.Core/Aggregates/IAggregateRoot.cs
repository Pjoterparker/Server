using System;
using System.Collections.Generic;
using PjoterParker.Core.Commands;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateRoot
    {
        IEnumerable<EventComposite> Events { get; }

        long Version { get; set; }

        Guid Id { get; set; }
    }
}