using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Core.Specification;

namespace PjoterParker.Core.Extensions
{
    public static class AggragateExtensions
    {
        public static void AddMethod<TAggregate, TEvent>(this Dictionary<string, Action<TAggregate, IEvent>> @that) where TAggregate : AggregateRoot<TAggregate>, IApply<TEvent> where TEvent : class, IEvent
        {
            @that.Add(typeof(TEvent).Name, (entity, @event) => { entity.Apply(@event as TEvent); });
        }

        public static void AddMethod<TAggregate, TEvent>(this Dictionary<string, Func<IComponentContext, IEvent, IValidator<TAggregate>>> @that) where TAggregate : AggregateRoot<TAggregate> where TEvent : class, IEvent
        {
            @that.Add(typeof(TEvent).Name, (context, @event) => { return context.Resolve<ISpecificationFor<TAggregate, TEvent>>().Apply(@event as TEvent); });
        }
    }
}
