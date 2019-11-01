using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class SpotEnabled : IEvent
    {
        public SpotEnabled(Guid spotId)
        {
            SpotId = spotId;
        }

        public Guid SpotId { get; set; }
    }
}