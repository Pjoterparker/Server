using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Api.Database.Entities
{
    public class Reservation
    {
        public long ReservationId { get; set; }

        public Guid SpotId { get; set; }

        public Guid UserId { get; set; }
    }
}
