using System;
using System.ComponentModel.DataAnnotations;
using PjoterParker.Events;

namespace PjoterParker.Api.Database.Entities
{
    public class Spot
    {
        public Spot()
        {
        }

        public Spot(SpotCreated @event)
        {
            SpotId = @event.SpotId;
            LocationId = @event.LocationId;
            Name = @event.Name;
        }

        public virtual Location Location { get; set; }

        public Guid LocationId { get; set; }

        public string Name { get; set; }

        [Key] public Guid SpotId { get; set; }

        public Guid? UserId { get; set; }
        public bool IsDisabled { get; set; }
    }
}