using System;
using System.Collections.Generic;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using PjoterParker.Common.Helpers;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;
using PjoterParker.Core.Specification;

namespace PjoterParker.Core.Aggregates
{
    public class AggregateRoot<TAggregate> : IAggregateRoot
        where TAggregate : AggregateRoot<TAggregate>,
        IApply<PropertyChanged<TAggregate>>
    {
        private readonly List<EventComposite> _events = new List<EventComposite>();

        [JsonIgnore]
        public IComponentContext Context { get; set; }

        [JsonIgnore]
        public IEnumerable<EventComposite> Events => _events;

        public Guid Id { get; set; }

        public long Version { get; set; } = -1;

        public void Apply(PropertyChanged<TAggregate> @event)
        {
            GetType().GetProperty(@event.PropertyName).SetValue(this, @event.NewValue);
        }

        public void Apply(IEvent @event)
        {
            //string eventName = @event.GetType().Name;
            //if (this is IApply<TEvent>)
            //{
            //    (this as IApply<TEvent>).Apply(@event);
            //}
        }

        protected void AddEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (this is IApply<TEvent>)
            {
                (this as IApply<TEvent>).Apply(@event);
            }

            if (Context.TryResolve(out ISpecificationFor<TAggregate, TEvent> specificationFor))
            {
                var validator = specificationFor.Apply(@event);
                ValidationResult result = validator.Validate(this);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors);
                }
            }

            _events.Add(new EventComposite(@event));
        }

        protected bool Compare<TType>(string propertyName, TType oldValue, TType newValue) where TType : class, IEquatable<TType>
        {
            if (!CompareHelper.ClassEquals(oldValue, newValue))
            {
                var propertyChangedEvent = new PropertyChanged<TAggregate>(
                    Id,
                    typeof(TAggregate).FullName,
                    typeof(TType).FullName,
                    propertyName,
                    oldValue?.ToString(),
                    newValue?.ToString());

                Apply(propertyChangedEvent);
                AddEvent(propertyChangedEvent);

                return true;
            }

            return false;
        }

        protected bool Compare<TType>(string propertyName, TType? oldValue, TType? newValue) where TType : struct
        {
            if (!CompareHelper.StructEquals(oldValue, newValue))
            {
                var propertyChangedEvent = new PropertyChanged<TAggregate>(
                    Id,
                    typeof(TAggregate).FullName,
                    typeof(TType).FullName,
                    propertyName,
                    oldValue?.ToString(),
                    newValue?.ToString());

                Apply(propertyChangedEvent);
                AddEvent(propertyChangedEvent);

                return true;
            }

            return false;
        }
    }
}