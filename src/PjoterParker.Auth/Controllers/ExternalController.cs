using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PjoterParker.Auth.Database;

namespace PjoterParker.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        private readonly IAuthenticationSchemeProvider _schemaProvider;

        private readonly IdentityServerTools _tools;

        public ExternalController(IAuthenticationSchemeProvider schemaProvider, IdentityServerTools tools, AuthDatabaseContext context)
        {
            _schemaProvider = schemaProvider;
            _tools = tools;
        }

        [AllowAnonymous]
        [HttpGet(nameof(Callback))]
        [Route("/signin-softserve")]
        public async Task<ActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            return RedirectToAction(nameof(Token));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(Challenge))]
        public IActionResult Challenge(string provider)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "scheme", provider },
                }
            };

            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(Providers))]
        public async Task<IActionResult> Providers()
        {
            var schemas = (await _schemaProvider.GetAllSchemesAsync()).Select(x => new { x.Name, x.DisplayName });
            return new JsonResult(schemas);
        }

        [Authorize(AuthenticationSchemes = IdentityServerConstants.ExternalCookieAuthenticationScheme)]
        [HttpGet(nameof(Token))]
        public async Task<string> Token()
        {
            return await _tools.IssueClientJwtAsync("testtttsaa", 5 * 60);
        }
    }
}
