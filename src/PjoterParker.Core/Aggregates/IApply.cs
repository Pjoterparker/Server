using PjoterParker.Core.Events;

namespace PjoterParker.Core.Aggregates
{
    public interface IApply<TEvent> where TEvent : IEvent
    {
        void Apply(TEvent @event);
    }
}
