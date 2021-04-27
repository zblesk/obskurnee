using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserServiceBase _users;

        public UserController(
           UserServiceBase users,
           ILogger<UserController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        [HttpGet]
        public Task<List<UserInfo>> GetAllUsers() => _users.GetAllUsers();

        [HttpGet]
        [Route("{email}")]
        public async Task<UserInfo> GetUser(string email) => await _users.GetUserByEmail(email);

        [HttpPost]
        [Route("{email}")]
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
        public async Task<UserInfo> SetAvatar([FromForm] IFormFile avatar)
        {
            using (var image = Image.Load(avatar.OpenReadStream(), out var format))
            using (var ms = new MemoryStream())
            {
                image.Mutate(img =>
                    img.Resize(new ResizeOptions { Mode = ResizeMode.Crop, Size = new Size(100, 100) }));
                image.Save(ms, format);
                await _users.SetUserAvatar(
                    User.GetUserId(),
                    ms.ToArray(),
                    Path.GetExtension(avatar.FileName));
                return await _users.GetUserById(User.GetUserId());
            }
        }
    }
}