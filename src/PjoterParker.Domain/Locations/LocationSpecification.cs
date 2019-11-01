using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database;
using PjoterParker.Core.Specification;
using PjoterParker.Core.Validation;
using PjoterParker.Events;

namespace PjoterParker.Domain.Locations
{
    public class LocationSpecification : AppAbstractValidation<LocationAggregate>,
        ISpecificationFor<LocationAggregate, LocationCreated>,
        ISpecificationFor<LocationAggregate, LocationUpdated>
    {
        private readonly IApiDatabaseContext _database;

        public LocationSpecification(IApiDatabaseContext database)
        {
            _database = database;
        }

        public IValidator<LocationAggregate> Apply(LocationCreated @event)
        {
            MusHaveName();
            MusHaveCity();
            MusHaveStreet();
            MustHaveUniqueName();
            return this;
        }

        public IValidator<LocationAggregate> Apply(LocationUpdated @event)
        {
            MusHaveName();
            MusHaveCity();
            MusHaveStreet();
            MustHaveExistingId();
            MustBeNotDisabled();
            MustHaveUniqueName();
            return this;
        }

        public void MusHaveCity()
        {
            RuleFor(x => x.City).NotEmpty().MaximumLength(255);
        }

        public void MusHaveName()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        }

        public void MusHaveStreet()
        {
            RuleFor(x => x.Street).NotEmpty().MaximumLength(255);
        }

        public void MustBeNotDisabled()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Location
                .AnyAsync(location => location.LocationId == aggregate.Id && location.IsDisabled, token);
                if (exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Location is disabled");
                }
            });
        }

        public void MustHaveExistingId()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Location.AnyAsync(location => location.LocationId == aggregate.Id, token);
                if (!exists)
                {
                    context.AddFailure(nameof(aggregate.Id), "Location with that Id doesn't exists");
                }
            });
        }

        public void MustHaveUniqueName()
        {
            RuleFor(x => x).CustomAsync(async (aggregate, context, token) =>
            {
                var exists = await _database.Location
                .AnyAsync(location => location.Name == aggregate.Name && location.LocationId != aggregate.Id, token);
                if (exists)
                {
                    context.AddFailure(nameof(aggregate.Name), "Location with that name already exists");
                }
            });
        }
    }
}