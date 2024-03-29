﻿using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class LocationEnabled : IEvent
    {
        public LocationEnabled(Guid locationId)
        {
            LocationId = locationId;
        }

        public Guid LocationId { get; set; }
    }
}