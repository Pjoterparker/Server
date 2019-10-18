using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateRoot
    {
        IComponentContext Context { get; set; }

        IEnumerable<EventComposite> Events { get; }

        Guid Id { get; set; }

        long Version { get; set; }

        void Apply(IEvent @event);

        Task CommitAsync(IAggregateStore aggregateStore);
    }
}
