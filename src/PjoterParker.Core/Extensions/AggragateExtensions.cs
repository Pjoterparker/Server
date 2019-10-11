using System;
using System.Collections.Generic;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Extensions
{
    public static class AggragateExtensions
    {
        public static void AddMethod<TAggregate, TEvent>(this Dictionary<string, Action<TAggregate, IEvent>> @that) where TAggregate : IAggregateRoot, IApply<TEvent> where TEvent : class, IEvent
        {
            @that.Add(typeof(TEvent).Name, (entity, @event) => { entity.Apply(@event as TEvent); });
        }
    }
}