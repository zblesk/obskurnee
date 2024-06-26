﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.IO;

namespace Obskurnee.Controllers;

[Authorize]
[Route("api/users")]
public class UserController : Controller
{
    private readonly UserServiceBase _users;
    readonly UserManager<Bookworm> _userManager;

    public UserController(
       UserServiceBase users
#if DEBUG
       , UserManager<Bookworm> userManager
#endif
       )
    {
        _users = users ?? throw new ArgumentNullException(nameof(users));
#if DEBUG
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
#endif
    }

    [HttpGet]
    public Task<List<UserInfo>> GetAllUsers() => _users.GetAllUsers();

    [HttpGet]
    [Route("{email}")]
    public async Task<UserInfo> GetUser(string email) => await _users.GetUserByEmail(email);

    [HttpPost]
    [Route("{email}")]
    [Authorize(Policy = "CanUpdate")]
    public Task<UserInfo> UpdateUser(string email, [FromBody] UserInfo updateInfo)
    {
        // todo: auth
        return _users.UpdateUserProfile(
            email,
            updateInfo.Name,
            updateInfo.Phone,
            updateInfo.GoodreadsUrl,
            updateInfo.AboutMe);
    }

    [HttpGet]
    [Route("language")]
    public Task<string> GetMyLanguage()
        => _users.GetUserLanguage(User.GetUserId());

    [HttpPost]
    [Route("language")]
    public Task SetMyLanguage([FromBody] JObject payload)
        => _users.SetUserLanguage(User.GetUserId(), payload["language"].ToString());

    [HttpPost]
    [Route("avatar")]
    [Authorize(Policy = "CanUpdate")]
    public async Task<UserInfo> SetAvatar([FromForm] IFormFile avatar)
    {
        using var image = Image.Load(avatar.OpenReadStream());
        var encoder = image.DetectEncoder(avatar.FileName);
        using var ms = new MemoryStream();
        image.Mutate(img =>
            img.Resize(new ResizeOptions { Mode = ResizeMode.Crop, Size = new Size(100, 100) }));
        image.Save(ms, encoder);
        await _users.SetUserAvatar(
            User.GetUserId(),
            ms.ToArray(),
            Path.GetExtension(avatar.FileName));
        return await _users.GetUserById(User.GetUserId());
    }

#if DEBUG
    [HttpGet]
    [Route("claims")]
    public IActionResult GetMyClaims()
        => Json(User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray());

    [HttpGet]
    [Route("roles")]
    public async Task<IActionResult> GetMyRoles()
    {
        var user = await _userManager.GetUserAsync(User);
        Trace.Assert(user != null);
        return Json(await _userManager.GetRolesAsync(user));
    }

#endif
}
