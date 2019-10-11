using System;
using System.Linq;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Domain.Spots;
using PjoterParker.Events;

namespace PjoterParker.Domain.Locations
{
    public class SpotAggregate : AggregateRoot<SpotAggregate>,
    IApply<SpotCreated>,
    IApply<OwnerAssignedToSpot>,
    IApply<MarkSpotAsUnoccupied>,
    IApply<PropertyChanged<SpotAggregate>>
    {
        public SpotAggregate()
        {
            //AddApplyMethod<SpotCreated>();
            //AddApplyMethod<OwnerAssignedToSpot>();
            //AddApplyMethod<MarkSpotAsUnoccupied>();

            AddSpecificationsMethod<SpotCreated>();
            AddSpecificationsMethod<SpotUpdated>();
        }

        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public Guid? OwnerId { get; set; }

        public void Apply(MarkSpotAsUnoccupied @event)
        {
            OwnerId = null;
        }

        public void Apply(OwnerAssignedToSpot @event)
        {
            OwnerId = @event.OwnerId;
        }

        public void Apply(SpotCreated @event)
        {
            Id = @event.SpotId;
            LocationId = @event.LocationId;
            Name = @event.Name;
        }

        public void Create(Guid spotId, CreateSpot.Command command)
        {
            AddEvent(new SpotCreated(spotId, command.Name, command.LocationId));

            if (command.OwnerId.HasValue)
            {
                AddEvent(new OwnerAssignedToSpot(spotId, command.OwnerId.Value));
            }
        }

        public void Update(UpdateSpot.Command command)
        {
            Compare(nameof(Name), Name, command.Name);

            if (OwnerId != command.OwnerId)
            {
                if (OwnerId.HasValue)
                {
                    AddEvent(new OwnerUnassignedFromSpot(Id, OwnerId.Value));
                }

                if (command.OwnerId.HasValue)
                {
                    AddEvent(new OwnerAssignedToSpot(Id, command.OwnerId.Value));
                }
                else
                {
                    AddEvent(new MarkSpotAsUnoccupied(Id));
                }
            }

            if (Events.Any())
            {
                AddEvent(new SpotUpdated(command.SpotId));
            }
        }
    }
}