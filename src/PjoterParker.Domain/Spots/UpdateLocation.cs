using System;
using System.Threading.Tasks;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Validation;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Domain.Spots
{
    public class UpdateSpot
    {
        public class Command : ICommand
        {
            public string Name { get; set; }

            public Guid? OwnerId { get; set; }

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
                spot.Update(command);
                return spot;
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            }
        }
    }
}
