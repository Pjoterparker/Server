using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Specification
{
    public interface ISpecificationFor<TAggregate, TEvent> where TEvent : IEvent where TAggregate : IAggregateRoot
    {
        IValidator<TAggregate> Apply(TEvent @event);
    }
}
