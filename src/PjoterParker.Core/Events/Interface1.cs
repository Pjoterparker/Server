using System.Threading.Tasks;

namespace PjoterParker.Core.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEvent @event);
    }
}
