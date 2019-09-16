using System;
using PjoterParker.Core.Aggregates;

namespace PjoterParker.Infrastructure
{
    public class TestAggragetRoot :
        AggregateRoot<TestAggragetRoot>,
        IAggregateState<TestEvent>,
        IAggregateState<TestEvent2>
    {
        public IApply<TestEvent> Applier => throw new NotImplementedException();

        IApply<TestEvent2> IAggregateState<TestEvent2>.Applier => throw new NotImplementedException();

        public void Apply(TestEvent @event)
        {
            Applier.Apply(@event);
        }

        public void Apply(TestEvent2 @event)
        {
            throw new NotImplementedException();
        }
    }
}