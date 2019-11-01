using System;
using System.Threading.Tasks;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Services;
using PjoterParker.Core.Validation;

namespace PjoterParker.Domain.Spots
{
    public class CreateSpot
    {
        public class Command : ICommand
        {
            public Guid LocationId { get; set; }

            public string Name { get; set; }

            public Guid? OwnerId { get; set; }
        }

        public class Handler : ICommandHandlerAsync<Command>
        {
            private readonly IAggregateStore _aggregateStore;

            private readonly IGuidService _guidService;

            public Handler(IGuidService guidService, IAggregateStore aggregateStore)
            {
                _guidService = guidService;
                _aggregateStore = aggregateStore;
            }

            public Task<IAggregateRoot> ExecuteAsync(Command command)
            {
                var spot = _aggregateStore.GetNew<SpotAggregate>();
                spot.Create(_guidService.New(), command);
                return Task.FromResult<IAggregateRoot>(spot);
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