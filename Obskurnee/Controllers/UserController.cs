using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
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
    }
}
