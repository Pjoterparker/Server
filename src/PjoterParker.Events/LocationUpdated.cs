using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class LocationUpdated : IEvent
    {
        public LocationUpdated(Guid locationId)
        {
            LocationId = locationId;
        }

        public Guid LocationId { get; set; }
    }
}
