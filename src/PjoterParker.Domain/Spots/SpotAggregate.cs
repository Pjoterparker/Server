using System;
using System.Collections.Generic;
using System.Linq;
using PjoterParker.Common.Aggregates;
using PjoterParker.Core.Aggregates;
using PjoterParker.Core.Events;
using PjoterParker.Events;

namespace PjoterParker.Domain.Spots
{
    public class SpotAggregate : AggregateRoot<SpotAggregate>,
    IApply<SpotCreated>,
    IApply<OwnerAssignedToSpot>,
    IApply<MarkSpotAsUnoccupied>,
    IApply<SpotEnabled>,
    IApply<SpotDisabled>,
    IApply<SpotAccessibleForReservation>,
    IApply<PropertyChanged<SpotAggregate>>
    {
        public SpotAggregate()
        {
        }

        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public Guid? OwnerId { get; set; }

        public bool IsDisabled { get; set; }

        public List<ReservationEntity> Reservations { get; set; } = new List<ReservationEntity>();

        public List<DateTime> Availability { get; set; } = new List<DateTime>();

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

        public void Enable()
        {
            if (IsDisabled)
            {
                AddEvent(new SpotEnabled(Id));
            }
        }

        public void Disable()
        {
            if (!IsDisabled)
            {
                AddEvent(new SpotEnabled(Id));
            }
        }

        public void Create(Guid spotId, CreateSpot.Command command)
        {
            AddEvent(new SpotCreated(spotId, command.Name, command.LocationId));

            if (command.OwnerId.HasValue)
            {
                AddEvent(new OwnerAssignedToSpot(spotId, command.OwnerId.Value));
            }
        }

        public void Reserve(ReserveSpot.Command command)
        {
            AddEvent(new ReservationMade(command.SpotId, command.From, command.To, command.UserId));
        }

        public void Cancel(Guid reservationId, CancelReservation.Command command)
        {
            AddEvent(new ReservationCanceled(reservationId, command.SpotId));
        }

        public void Vacate(VacateSpot.Command command)
        {
            AddEvent(new SpotAccessibleForReservation(command.SpotId, command.From, command.To));
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

        public void Apply(SpotEnabled @event)
        {
            IsDisabled = false;
        }

        public void Apply(SpotDisabled @event)
        {
            IsDisabled = true;
        }

        public void Apply(SpotAccessibleForReservation @event)
        {
            for(var date = @event.From.Date; date < @event.To.Date; date = date.AddDays(1))
            {
                if (!Availability.Contains(date))
                {
                    Availability.Add(date);
                }
            }
        }
    }
}