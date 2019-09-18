using System.Threading.Tasks;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Services;
using PjoterParker.Core.Validation;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Api.Controllers.Locations
{
    public class CreateLocation
    {
        public class Command : ICommand
        {
            public string City { get; set; }

            public string Name { get; set; }

            public string Street { get; set; }
        }

        public class Handler : ICommandHandlerAsync<Command>
        {
            private readonly IGuidService _guidService;

            private readonly IAggregateStore _aggregateStore;

            public Handler(IGuidService guidService, IAggregateStore aggregateStore)
            {
                _guidService = guidService;
                _aggregateStore = aggregateStore;
            }

            public Task<IAggregateRoot> ExecuteAsync(Command command)
            {
                var location = _aggregateStore.GetNew<LocationAggregate>();
                location.Create(_guidService.New(), command);
                return Task.FromResult<IAggregateRoot>(location);
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