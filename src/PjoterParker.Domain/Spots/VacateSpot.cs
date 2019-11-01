using System;
using System.Threading.Tasks;
using FluentValidation;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Commands;
using PjoterParker.Core.Validation;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Domain.Spots
{
    public class VacateSpot
    {
        public class Command : ICommand
        {
            public Guid SpotId { get; set; }
            public Guid UserId { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
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
                spot.Vacate(command);
                return spot;
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            public Validator()
            {
                RuleFor(x => x.To.Date).GreaterThan(x => x.From.Date);
                RuleFor(x => x).Must(x => (x.To.Date - x.From.Date).TotalDays <= 30).WithMessage("Date diffrence must be less than 30 days");
            }
        }
    }
}