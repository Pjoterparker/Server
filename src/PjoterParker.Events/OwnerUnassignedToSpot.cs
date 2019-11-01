using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class OwnerUnassignedFromSpot : IEvent
    {
        public OwnerUnassignedFromSpot(Guid spotId, Guid ownerId)
        {
            SpotId = spotId;
            OldOwnerId = ownerId;
        }

        public Guid OldOwnerId { get; set; }

        public Guid SpotId { get; set; }
    }
}