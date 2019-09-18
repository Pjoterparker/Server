using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PjoterParker.Api.Controllers.Locations;
using PjoterParker.Core.Commands;

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
        public async Task Post(CreateLocation.Command value)
        {
            await _commandDispatcher.DispatchAsync(value);
        }

        //[HttpPut]
        //public async Task Put(UpdateLocation.Command value)
        //{
        //    await _commandDispatcher.DispatchAsync(value);
        //}
    }
}