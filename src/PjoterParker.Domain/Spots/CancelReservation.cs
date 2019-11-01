using System;
using PjoterParker.Core.Commands;

namespace PjoterParker.Domain.Spots
{
    public class CancelReservation
    {
        public class Command : ICommand
        {
            public Guid SpotId { get; set; }
            public Guid UserId { get; set; }
        }
    }
}