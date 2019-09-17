using System;
using System.Collections.Generic;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Aggregates
{
    public class AggregateRoot<TAggregate> : IAggregateRoot
        where TAggregate : AggregateRoot<TAggregate>
    {
        private readonly List<EventComposite> _events = new List<EventComposite>();

        protected Dictionary<string, Action<TAggregate, IEvent>> _applyMethods = new Dictionary<string, Action<TAggregate, IEvent>>();

        protected Dictionary<string, Func<IComponentContext, IEvent, IValidator<TAggregate>>> _specifications = new Dictionary<string, Func<IComponentContext, IEvent, IValidator<TAggregate>>>();

        public IComponentContext Context { get; set; }

        public IEnumerable<EventComposite> Events => _events;

        public Guid Id { get; set; }

        public long Version { get; set; }

        protected void AddEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            string eventName = @event.GetType().Name;

            if (_applyMethods.ContainsKey(eventName))
            {
                _applyMethods[eventName](this as TAggregate, @event);
            }

            if (_specifications.ContainsKey(eventName))
            {
                var validator = _specifications[eventName](Context, @event);
                ValidationResult result = validator.Validate(this);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors);
                }
            }

            _events.Add(new EventComposite(@event));
        }
    }
}
