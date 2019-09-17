using System.Threading.Tasks;

namespace PjoterParker.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
