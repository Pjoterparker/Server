using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Commands;
using PjoterParker.Domain.Locations;

namespace PjoterParker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public LocationsController(ICommandDispatcher commandDispatcher)
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
    }
}