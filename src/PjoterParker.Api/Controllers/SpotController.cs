using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Commands;
using PjoterParker.Domain.Locations;
using PjoterParker.Domain.Spots;

namespace PjoterParker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public SpotController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task CreateSpot(CreateSpot.Command value)
        {
            await _commandDispatcher.DispatchAsync(value);
        }

        [HttpPut]
        public async Task UpdateSpot(UpdateSpot.Command value)
        {
            await _commandDispatcher.DispatchAsync(value);
        }

        [HttpPost]
        [Route("{spotId}/Enable")]
        public async Task Enable(Guid spotId)
        {
            await _commandDispatcher.DispatchAsync(new EnableSpot.Command(spotId));
        }

        [HttpPost]
        [Route("{spotId}/Disable")]
        public async Task Disable(Guid spotId)
        {
            await _commandDispatcher.DispatchAsync(new DisableSpot.Command(spotId));
        }
    }
}
