using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class SpotAccessibleForReservation : IEvent
    {
        public Guid SpotId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public SpotAccessibleForReservation(Guid spotId, DateTime from, DateTime to)
        {
            SpotId = spotId;
            From = from;
            To = to;
        }
    }
}