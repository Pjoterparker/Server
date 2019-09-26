using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class OwnerAssignedToSpot : IEvent
    {
        public OwnerAssignedToSpot(Guid spotId, Guid ownerId)
        {
            SpotId = spotId;
            OwnerId = ownerId;
        }

        public Guid OwnerId { get; set; }

        public Guid SpotId { get; set; }
    }
}
