using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class ReservationCanceled : IEvent
    {
        public ReservationCanceled(Guid reservationId, Guid spotId)
        {
            ReservationId = reservationId;
            SpotId = spotId;
        }

        public Guid ReservationId { get; set; }

        public Guid SpotId { get; set; }
    }
}