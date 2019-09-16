using System;
using System.Collections.Generic;
using System.Text;
using PjoterParker.Core.Events;

namespace PjoterParker.Infrastructure
{
    public class TestEvent : IEvent
    {
        public int Id { get; set; }
    }

    public class TestEvent2 : IEvent
    {
        public int Id { get; set; }
    }
}
