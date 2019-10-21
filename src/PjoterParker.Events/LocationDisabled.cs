using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class LocationDisabled : IEvent
    {
        public LocationDisabled(Guid locationId)
        {
            LocationId = locationId;
        }

        public Guid LocationId { get; set; }
    }
}
