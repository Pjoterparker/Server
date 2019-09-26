using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class SpotCreated : IEvent
    {
        public SpotCreated(Guid spotId, string name, Guid locationId)
        {
            SpotId = spotId;
            Name = name;
            LocationId = locationId;
        }

        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public Guid SpotId { get; set; }
    }
}
