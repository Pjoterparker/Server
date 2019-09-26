using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class SpotUpdated : IEvent
    {
        public SpotUpdated(Guid spotId)
        {
            SpotId = spotId;
        }

        public Guid SpotId { get; set; }
    }
}
