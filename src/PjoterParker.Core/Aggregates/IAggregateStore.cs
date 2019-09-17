using System;
using System.Threading.Tasks;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateStore
    {
        Task<TAggregate> GetByIdAsync<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot<TAggregate>, new();

        Task<TAggregate> GetByIdAsync<TAggregate>(Guid aggregateId, int version) where TAggregate : AggregateRoot<TAggregate>, new();

        TAggregate GetNew<TAggregate>() where TAggregate : AggregateRoot<TAggregate>, new();

        Task SaveAsync(IAggregateRoot aggregate);
    }
}
