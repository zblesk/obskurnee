using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Obskurnee.Controllers
{
    [Route("api/accounts")]
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
            var user = await _users.Register(creds);
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

        [HttpGet]
        [Authorize]
        public IEnumerable<UserInfo> GetAllUsers() => _users.Users.Values;

        [HttpPost("passwordreset/{userId}/{resetToken}")]
        [AllowAnonymous]
        public Task<IdentityResult> ResetPassword(string userId, string resetToken, [FromBody] JsonElement payload)
            => _users.ResetPassword(
                HttpUtility.UrlDecode(userId),
                HttpUtility.UrlDecode(resetToken),
                payload.GetProperty("password").GetString());

        [HttpGet("passwordreset/{email}")]
        [AllowAnonymous]
        public Task<bool> InitiatePasswordReset(string email) => _users.InitiatePasswordReset(email);
    }
}
