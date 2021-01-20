using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<Bookworm> _signInManager;
        private readonly UserManager<Bookworm> _userManager;
        private readonly ILogger _logger;

        private static readonly SigningCredentials SigningCreds = new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly UserService _users;

        public AccountController(
           UserManager<Bookworm> userManager,
           SignInManager<Bookworm> signInManager,
           UserService users,
           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        //    [Authorize(Policy = "ModOnly")]
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
