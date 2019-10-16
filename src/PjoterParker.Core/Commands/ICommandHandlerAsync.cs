using System.Collections.Generic;
using System.Threading.Tasks;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;

namespace PjoterParker.Core.Commands
{
    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<IEnumerable<EventComposite>> ExecuteAsync(TCommand command);
    }
}
