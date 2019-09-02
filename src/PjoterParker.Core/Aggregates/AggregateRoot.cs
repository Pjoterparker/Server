using System;
using System.Collections.Generic;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Aggregates
{
    public class AggregateRoot<TAggregate, TIdentity> : IAggregateRoot<TIdentity>
        where TAggregate : AggregateRoot<TAggregate, TIdentity>
    {
        private static Dictionary<string, Action<TAggregate, IEvent>> _applyMethods = new Dictionary<string, Action<TAggregate, IEvent>>();

        public TIdentity Id { get; }

        private readonly List<EventComposite> _events = new List<EventComposite>();

        public IEnumerable<EventComposite> Events => _events;

        public long Version { get; private set; }
    }
}