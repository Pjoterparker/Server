using System;
using System.Threading.Tasks;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;

namespace PjoterParker.Domain.Spots
{
    public class DisableSpot
    {
        public class Command : ICommand
        {
            public Command(Guid spotId)
            {
                SpotId = spotId;
            }

            public Guid SpotId { get; set; }
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
                var spot = await _aggregateStore.GetByIdAsync<SpotAggregate>(command.SpotId);
                spot.Disable();
                return spot;
            }
        }
    }
}