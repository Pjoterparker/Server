using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Commands;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public LocationController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task CreateLocation(CreateLocation.Command value)
        {
            await _commandDispatcher.DispatchAsync(value);
        }

        [HttpPut]
        public async Task UpdateLocation(UpdateLocation.Command value)
        {
            await _commandDispatcher.DispatchAsync(value);
        }

        [HttpPost]
        [Route("{locationId}/Enable")]
        public async Task Enable(Guid locationId)
        {
            await _commandDispatcher.DispatchAsync(new EnableLocation.Command(locationId));
        }

        [HttpPost]
        [Route("{locationId}/Disable")]
        public async Task Disable(Guid locationId)
        {
            await _commandDispatcher.DispatchAsync(new DisableLocation.Command(locationId));
        }
    }
}
