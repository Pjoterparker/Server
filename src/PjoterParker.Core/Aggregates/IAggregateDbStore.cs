using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateDbStore
    {
        Task SaveAsync(IAggregateRoot aggregate);
    }
}
