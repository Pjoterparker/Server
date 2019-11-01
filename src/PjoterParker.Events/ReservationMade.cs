using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class ReservationMade : IEvent
    {
        public ReservationMade(Guid spotId, DateTime from, DateTime to, Guid userId)
        {
            SpotId = spotId;
            From = from;
            To = to;
            UserId = userId;
        }

        public Guid SpotId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid UserId { get; }
    }
}