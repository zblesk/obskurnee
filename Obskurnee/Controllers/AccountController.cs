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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        private static readonly SigningCredentials SigningCreds = new SigningCredentials(Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public AccountController(
            UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("login")]
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
                role = principal.FindFirstValue(ClaimTypes.Role)
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

        // On a real project, you would use the SignInManager 
        // to locate the user by its email and build its ClaimsPrincipal:
        //  var user = await _signInManager.UserManager.FindByEmailAsync(email);
        //  var principal = await _signInManager.CreateUserPrincipalAsync(user)
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
                role = this.User?.FindFirstValue(ClaimTypes.Role),
            });
        }

        [HttpPost("register")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<JsonResult> Register([FromBody] LoginCredentials creds)
        {
            var user = new ApplicationUser { UserName = creds.Email, Email = creds.Email };
            var result = await _userManager.CreateAsync(user, creds.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                // await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "nerd"));
                _logger.LogInformation("User created a new account with password.");
                return Context();
            }
            return new JsonResult(result);
        }

        [HttpPost("makeadmin")]
        public async Task MakeAdmin([FromBody] LoginCredentials creds)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(creds.Email);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "admin"));
        }


        [HttpPost("ao")]
        [Authorize(Policy = "AdminOnly")]
        public async Task MakeAdminasd()
        {
            var aas = 433434;
        }
    }
}
