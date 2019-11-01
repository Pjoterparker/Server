using System;

namespace PjoterParker.Domain.Spots
{
    public class ReservationEntity
    {
        public DateTime ReservationDate { get; set; }

        public Guid UserId { get; set; }
    }
}