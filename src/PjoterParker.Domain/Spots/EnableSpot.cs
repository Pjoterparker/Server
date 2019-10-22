using System;
using System.Threading.Tasks;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Domain.Spots
{
    public class EnableSpot
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

            public Task<IAggregateRoot> ExecuteAsync(Command command)
            {
                var spot = _aggregateStore.GetNew<SpotAggregate>();
                return Task.FromResult<IAggregateRoot>(spot);
            }
        }
    }
}