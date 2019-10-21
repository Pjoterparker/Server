using System;
using System.Linq;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Events;

namespace PjoterParker.Domain.Locations
{
    public class LocationAggregate : AggregateRoot<LocationAggregate>,
      IApply<LocationCreated>,
      IApply<PropertyChanged<LocationAggregate>>,
      IApply<LocationEnabled>,
      IApply<LocationDisabled>,
      IApply<LocationUpdated>
    {
        public LocationAggregate()
        {
        }

        public string City { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public bool IsDisabled { get; set; }

        public void Apply(LocationCreated @event)
        {
            Id = @event.LocationId;
            Name = @event.Name;
            City = @event.City;
            Street = @event.Street;
        }

        public void Enable()
        {
            if (IsDisabled)
            {
                AddEvent(new LocationEnabled(Id));
            }
        }

        public void Disable()
        {
            if (!IsDisabled)
            {
                AddEvent(new LocationDisabled(Id));
            }
        }

        public void Create(Guid locationId, CreateLocation.Command command)
        {
            var locationCreated = new LocationCreated(locationId, command.Name, command.City, command.Street);
            AddEvent(locationCreated);
        }

        public void Update(UpdateLocation.Command command)
        {
            Compare(nameof(Name), Name, command.Name);
            Compare(nameof(City), City, command.City);
            Compare(nameof(Street), Street, command.Street);

            if (Events.Any())
            {
                AddEvent(new LocationUpdated(command.LocationId));
            }
        }

        public void Apply(LocationEnabled @event)
        {
            IsDisabled = false;
        }

        public void Apply(LocationDisabled @event)
        {
            IsDisabled = true;
        }

        public void Apply(LocationUpdated @event)
        {
        }
    }
}