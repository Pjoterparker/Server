using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using PjoterParker.Core.Specification;
using PjoterParker.Core.Validation;
using PjoterParker.Database;
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

        public void MustHaveExistingId()
        {
            RuleFor(x => x).CustomAsync(async (command, context, token) =>
            {
                //var exists = await _database.Location.AnyAsync(location => location.LocationId == command.Id, token);
                //if (!exists)
                //{
                //    context.AddFailure(nameof(command.Id), "Location with that Id doesn't exists");
                //}
            });
        }

        public void MustHaveUniqueName()
        {
            RuleFor(x => x).CustomAsync(async (command, context, token) =>
            {
                //bool doesNameIsAlreadyInUseByAnotherLocation = await _database.Location
                //.AnyAsync(location => location.Name == command.Name && location.LocationId != command.Id, token);

                //if (doesNameIsAlreadyInUseByAnotherLocation)
                //{
                //    context.AddFailure(nameof(command.Name), "Location with that Name already exists");
                //}
            });
        }
    }
}
