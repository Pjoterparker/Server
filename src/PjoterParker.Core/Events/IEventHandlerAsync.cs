using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PjoterParker.Core.Events
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
