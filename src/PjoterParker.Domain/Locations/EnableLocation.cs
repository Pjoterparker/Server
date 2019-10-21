using System;
using System.Threading.Tasks;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;

namespace PjoterParker.Domain.Locations
{
    public class EnableLocation
    {
        public class Command : ICommand
        {
            public Command(Guid locationId)
            {
                LocationId = locationId;
            }

            public Guid LocationId { get; set; }
        }

        public class Handler : ICommandHandlerAsync<Command>
        {
            private readonly IAggregateStore _aggregateStore;

            public Handler(IAggregateStore aggregateStore)
            {
                _aggregateStore = aggregateStore;
            }

            public async Task<IAggregateRoot> ExecuteAsync(Command command)
            {
                var location = await _aggregateStore.GetByIdAsync<LocationAggregate>(command.LocationId);
                location.Enable();
                return location;
            }
        }
    }
}