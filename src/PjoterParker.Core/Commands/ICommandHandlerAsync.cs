using System.Threading.Tasks;
using PjoterParker.Core.Aggregates;

namespace PjoterParker.Core.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IAggregateRoot> ExecuteAsync(TCommand command);
    }
}