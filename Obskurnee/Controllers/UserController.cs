using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserService _users;

        public UserController(
           UserService users,
           ILogger<UserController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        [HttpGet]
        public JsonResult GetAllUsers() => Json(_users.GetAllUsers(true));

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
    }
}
