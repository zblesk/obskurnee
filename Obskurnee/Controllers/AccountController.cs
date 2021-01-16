using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
            if (!await ValidateLogin(creds))
            {
                return Unauthorized(new
                {
                    error = "Login failed"
                });
            }
            var principal = await GetPrincipal(creds);
            var token = new JwtSecurityToken(
                "obskurnee",
                "obskurnee",
                principal.Claims,
                expires: DateTime.UtcNow.AddDays(190),
                signingCredentials: SigningCreds);

            return Json(new
            {
                token = _tokenHandler.WriteToken(token),
                name = principal.Identity.Name,
                email = principal.FindFirstValue(ClaimTypes.Email),
                isAdmin = this.User?.FindFirstValue(BookclubClaims.Admin) ?? "false",
            });
        }

        private async Task<bool> ValidateLogin(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            if (user == null)
            {
                return false;
            }
            var signInRes = await _signInManager.CheckPasswordSignInAsync(user, creds.Password, lockoutOnFailure: false);
            return signInRes.Succeeded;
        }

        private async Task<ClaimsPrincipal> GetPrincipal(LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        [HttpGet("context")]
        public JsonResult Context()
        {
            return Json(new
            {
                name = this.User?.Identity?.Name,
                email = this.User?.FindFirstValue(ClaimTypes.Email),
                isAdmin = this.User?.FindFirstValue(BookclubClaims.Admin) ?? "false",
            });
        }

        [HttpPost("register")]
        //    [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Register([FromBody] LoginCredentials creds)
        {
            var user = new Bookworm
            {
                UserName = creds.Email.Substring(0, creds.Email.IndexOf('@')),
                Email = creds.Email,
                GoodreadsProfileUrl = "https://poeknasjkha url kekes"
            };
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                // await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                _logger.LogInformation("User created a new account with password.");
                _users.ReloadCache();
                return Json(user);
            }
            return ValidationProblem("Registration failed");
        }

        [HttpPost("makeadmin")]
        public async Task MakeAdmin([FromBody] LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            await _userManager.AddClaimAsync(user, new Claim(BookclubClaims.Admin, "true"));
        }


        [HttpPost("ao")]
        [Authorize(Policy = "AdminOnly")]
        public async Task MakeAdminasd()
        {
            var aas = 433434;
        }
    }
}
