using System;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class LocationCreated : IEvent
    {
        public LocationCreated(Guid locationId, string name, string city, string street)
        {
            LocationId = locationId;
            Name = name;
            City = city;
            Street = street;
        }

        public string City { get; set; }

        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }
    }
}