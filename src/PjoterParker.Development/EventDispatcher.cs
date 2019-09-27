using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using PjoterParker.Core.Events;
using PjoterParker.Database;

namespace PjoterParker.Development
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IComponentContext _context;

        public EventDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task DispatchAsync(IEvent @event)
        {
            foreach (var handler in GetHandlers(@event.GetType()))
            {
                await ((dynamic)handler).HandleAsync((dynamic)@event);
            }
        }

        private IEnumerable GetHandlers(Type eventType)
        {
            return (IEnumerable)_context.Resolve(typeof(IEnumerable<>).MakeGenericType(typeof(IEventHandlerAsync<>).MakeGenericType(eventType)));
        }
    }
}
