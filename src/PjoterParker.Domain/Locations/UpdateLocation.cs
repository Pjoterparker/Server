using System;
using System.Threading.Tasks;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Validation;

namespace PjoterParker.Domain.Locations
{
    public class UpdateLocation
    {
        public class Command : ICommand
        {
            public string City { get; set; }

            public Guid LocationId { get; set; }

            public string Name { get; set; }

            public string Street { get; set; }
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
                location.Update(command);
                return location;
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Street).NotEmpty().MaximumLength(255);
                RuleFor(x => x.City).NotEmpty().MaximumLength(255);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            }
        }
    }
}