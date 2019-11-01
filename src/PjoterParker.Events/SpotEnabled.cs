using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Events;

namespace PjoterParker.Events
{
    public class SpotEnabled : IEvent
    {
        public SpotEnabled(Guid spotId)
        {
            SpotId = spotId;
        }

        public Guid SpotId { get; set; }
    }
}
