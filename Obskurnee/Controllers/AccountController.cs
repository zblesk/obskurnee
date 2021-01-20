using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserService _users;

        public AccountController(
           UserService users,
           ILogger<AccountController> logger)
        {
            _logger = logger;
            _users = users;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody] LoginCredentials creds)
        {
            if (!await _users.ValidateLogin(creds))
            {
                return Unauthorized(new
                {
                    error = "Login failed"
                });
            }
            var principal = await _users.GetPrincipal(creds);
            var token = _users.GetToken(principal);

            return Json(UserInfo.FromPrincipal(principal, token));
        }


        [HttpGet("context")]
        public JsonResult Context() => Json(UserInfo.FromPrincipal(this.User));

        [HttpPost("register")]
        [Authorize(Policy = "ModOnly")]
        public async Task<IActionResult> Register([FromBody] LoginCredentials creds)
        {
            var user = _users.Register(creds);
            if (user != null)
            {
                return Json(user);
            }
            return ValidationProblem("Registration failed");
        }

        [HttpPost("makemoderator")]
        [Authorize(Policy = "ModOnly")]
        public async Task MakeModerator([FromBody] LoginCredentials creds)
            => await _users.MakeModerator(creds.Email);

        [HttpPost("registerfirstadmin")]
        [AllowAnonymous]
        public async Task<JsonResult> RegisterFirstAdmin([FromBody] LoginCredentials creds)
            => Json(await _users.RegisterFirstAdmin(creds));
    }
}
