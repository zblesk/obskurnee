using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Web;

namespace Obskurnee.Controllers;

[Route("api/accounts")]
public class AccountController(
   UserServiceBase users) : Controller
{
    private readonly UserServiceBase _users = users;

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
        var (user, err) = await _users.Register(creds);
        if (user != null)
        {
            return Json(user);
        }
        return ValidationProblem("Registration failed: " + err);
    }

    [HttpPost("registerfirstadmin")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterFirstAdmin([FromBody] LoginCredentials creds)
    {
        var (user, err) = await _users.RegisterFirstAdmin(creds);
        if (user != null)
        {
            return Json(user);
        }
        return ValidationProblem("Registration failed: " + err);
    }

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
