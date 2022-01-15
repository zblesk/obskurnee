using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Linq;

using System.Threading.Tasks;
using System.Web;

namespace Obskurnee.Controllers
{
    [Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserServiceBase _users;

        public AccountController(
           UserServiceBase users,
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

            return Json(UserInfo.From(principal, token));
        }

        [HttpGet("context")]
        public JsonResult Context() => Json(UserInfo.From(this.User));

        [HttpPost("register")]
        [Authorize(Policy = "ModOnly")]
        public async Task<IActionResult> Register([FromBody] LoginCredentials creds)
        {
            var user = await _users.Register(creds);
            if (user != null)
            {
                return Json(user);
            }
            return ValidationProblem("Registration failed");
        }

        [HttpPost("registerfirstadmin")]
        [AllowAnonymous]
        public async Task<JsonResult> RegisterFirstAdmin([FromBody] LoginCredentials creds)
            => Json(await _users.RegisterFirstAdmin(creds));

        [HttpPost("passwordreset/token/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, [FromBody] JObject payload)
        {
            var result = await _users.ResetPassword(
                HttpUtility.UrlDecode(userId),
                payload["resetToken"].ToString(),
                payload["password"].ToString());
            if (!result.Succeeded)
            {
                return StatusCode(403, result.Errors.Aggregate("", (a, e) => $"{a}{e.Description} "));
            }
            return Ok();
        }

        [HttpPost("passwordreset/{email}")]
        [AllowAnonymous]
        public Task<bool> InitiatePasswordReset(string email) => _users.InitiatePasswordReset(email);
    }
}
