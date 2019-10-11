using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateMap<TAggregate> where TAggregate : IAggregateRoot
    {
        void Save(TAggregate aggregate);
    }
}
