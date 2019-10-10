﻿using System;
using System.Threading.Tasks;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateStore
    {
        Task<TAggregate> GetByIdAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregateRoot, new();

        Task<TAggregate> GetByIdAsync<TAggregate>(Guid aggregateId, int version) where TAggregate : IAggregateRoot, new();

        TAggregate GetNew<TAggregate>() where TAggregate : IAggregateRoot, new();

        Task SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregateRoot;
    }

    public interface IAggregateStore<TAggregate> where TAggregate : IAggregateRoot
    {
        Task SaveAsync(TAggregate aggregate);
    }
}
