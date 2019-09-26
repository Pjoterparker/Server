using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PjoterParker.Core.Application;
using PjoterParker.Core.Specification;
using PjoterParker.Core.Validation;
using PjoterParker.Database;
using PjoterParker.Events;

namespace PjoterParker.Domain.Locations
{
    public class SpotSpecification : AppAbstractValidation<SpotAggregate>,
        ISpecificationFor<SpotAggregate, SpotCreated>,
        ISpecificationFor<SpotAggregate, SpotUpdated>
    {
        private readonly IApiDatabaseContext _database;

        private readonly IUniquenessService _uniquenessService;

        public SpotSpecification(IApiDatabaseContext database, IUniquenessService uniquenessService)
        {
            _database = database;
            _uniquenessService = uniquenessService;
        }

        public IValidator<SpotAggregate> Apply(SpotCreated @event)
        {
            MusHaveName();
            MustHaveExistingLocationId();
            MustHaveUniqueName();
            return this;
        }

        public IValidator<SpotAggregate> Apply(SpotUpdated @event)
        {
            MusHaveName();
            MustHaveExistingId();
            MustHaveExistingLocationId();
            MustHaveUniqueName();
            return this;
        }

        public void MusHaveName()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        }

        public void MustHaveExistingId()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Spot.AnyAsync(spot => spot.SpotId == aggregate.Id, token);
                if (!exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Spot with that Id doesn't exists");
                }
            });
        }

        public void MustHaveExistingLocationId()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Location.AnyAsync(location => location.LocationId == aggregate.LocationId, token);
                if (!exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Location with that Id doesn't exists");
                }
            });
        }

        public void MustHaveUniqueName()
        {
            RuleFor(x => x).Custom((aggregate, context) =>
            {
                var doesNameIsUnique = _uniquenessService.IsUnique(aggregate.Id, "spotName", aggregate.Name);

                if (!doesNameIsUnique)
                {
                    context.AddFailure(nameof(aggregate.Name), "Spot with that Name already exists");
                }
            });
        }
    }
}
