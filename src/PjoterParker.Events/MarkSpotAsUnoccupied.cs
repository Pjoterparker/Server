using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class MarkSpotAsUnoccupied : IEvent
    {
        public MarkSpotAsUnoccupied(Guid spotId)
        {
            SpotId = spotId;
        }

        public Guid SpotId { get; set; }
    }
}
