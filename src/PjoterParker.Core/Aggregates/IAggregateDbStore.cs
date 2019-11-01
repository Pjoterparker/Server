using System.Threading.Tasks;

namespace PjoterParker.Core.Aggregates
{
    public interface IAggregateDbStore
    {
        Task SaveAsync(IAggregateRoot aggregate);
    }
}