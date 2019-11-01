using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PjoterParker.Events;

namespace PjoterParker.Api.Database.Entities
{
    public class Location
    {
        public Location()
        {
        }

        public Location(LocationCreated @event)
        {
            LocationId = @event.LocationId;
            City = @event.City;
            Name = @event.Name;
            Street = @event.Street;
        }

        public string City { get; set; }

        [Key] public Guid LocationId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Spot> Spots { get; set; } = new HashSet<Spot>();

        public string Street { get; set; }

        public bool IsDisabled { get; set; }
    }
}