using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var derp = new LocationAggregate();
            derp.Apply(new Events.LocationCreated(Guid.NewGuid(), "ASdasda", "asdasda", "Adasda"));

            var xxx = JsonConvert.SerializeObject(derp);
        }
    }
}
