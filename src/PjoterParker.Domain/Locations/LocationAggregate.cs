using System;
using System.Runtime.Serialization;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Extensions;
using PjoterParker.Events;

namespace PjoterParker.Domain.Locations
{
    [DataContract]
    public class LocationAggregate : AggregateRoot<LocationAggregate>,
      IApply<LocationCreated>,
      IApply<LocationUpdated>
    {
        public LocationAggregate()
        {
            _specifications.AddMethod<LocationAggregate, LocationCreated>();
            _specifications.AddMethod<LocationAggregate, LocationUpdated>();

            _applyMethods.AddMethod<LocationAggregate, LocationCreated>();
            _applyMethods.AddMethod<LocationAggregate, LocationUpdated>();
        }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Street { get; set; }

        public void Apply(LocationCreated @event)
        {
            Id = @event.LocationId;
            Name = @event.Name;
            City = @event.City;
            Street = @event.Street;
        }

        public void Apply(LocationUpdated @event)
        {
        }

        public void Create(Guid aggregateId, CreateLocation.Command command)
        {
            var locationCreated = new LocationCreated(aggregateId, command.Name, command.City, command.Street);
            AddEvent(locationCreated);
        }

        //public void Update(UpdateLocationBase.Command command)
        //{
        //    Compare(nameof(Name), Name, command.Name);
        //    Compare(nameof(City), City, command.City);
        //    Compare(nameof(Street), Street, command.Street);

        //    if (Events.Any())
        //    {
        //        AddEvent(new LocationUpdated(command.LocationId));
        //    }
        //}
    }
}
