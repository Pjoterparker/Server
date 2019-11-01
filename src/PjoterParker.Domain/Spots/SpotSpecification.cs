using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database;
using PjoterParker.Core.Specification;
using PjoterParker.Core.Validation;
using PjoterParker.Events;

namespace PjoterParker.Domain.Spots
{
    public class SpotSpecification : AppAbstractValidation<SpotAggregate>,
        ISpecificationFor<SpotAggregate, SpotCreated>,
        ISpecificationFor<SpotAggregate, SpotUpdated>,
        ISpecificationFor<SpotAggregate, SpotAccessibleForReservation>
    {
        private readonly IApiDatabaseContext _database;

        public SpotSpecification(IApiDatabaseContext database)
        {
            _database = database;
        }

        public IValidator<SpotAggregate> Apply(SpotCreated @event)
        {
            MusHaveName();
            MustHaveExistingNotDisabledLocationId();
            MustHaveUniqueName();
            return this;
        }

        public IValidator<SpotAggregate> Apply(SpotUpdated @event)
        {
            MusHaveName();
            MustHaveExistingNotDisabledLocationId();
            MustNotBeDisabled();
            MustHaveUniqueName();
            return this;
        }

        public IValidator<SpotAggregate> Apply(SpotAccessibleForReservation @event)
        {
            MustNotBeDisabled();
            MustHaveExistingNotDisabledLocationId();
            return this;
        }

        public void MusHaveName()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        }

        public void MustNotBeDisabled()
        {
            RuleFor(x => x).Custom( (aggregate, context) =>
            {
                if (aggregate.IsDisabled)
                {
                    context.AddFailure(nameof(aggregate.Id), "Spot is disabled");
                }
            });
        }

        public void MustHaveExistingNotDisabledLocationId()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Location.AnyAsync(location =>
                                                               location.LocationId == aggregate.LocationId
                                                               && !location.IsDisabled, token);
                if (!exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Location with that Id doesn't exists or is disabled");
                }
            });
        }

        public void MustHaveUniqueName()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Spot.AnyAsync(spot => spot.SpotId != aggregate.Id && spot.Name == aggregate.Name, token);
                if (exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Spot with that Name already exists");
                }
            });
        }
    }
}