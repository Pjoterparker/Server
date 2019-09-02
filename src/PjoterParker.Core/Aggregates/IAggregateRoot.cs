using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateRoot
    {
        IEnumerable<EventComposite> Events { get; }

        long Version { get; }
    }

    public interface IAggregateRoot<out TIdentity> : IAggregateRoot
    {
        TIdentity Id { get; }
    }
}
