using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserService _users;
        private readonly UserManager<Bookworm> _userManager;

        public UserController(
           UserService users,
           ILogger<UserController> logger,
           UserManager<Bookworm> userManager)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _users = users ?? throw new System.ArgumentNullException(nameof(users));
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public IEnumerable<UserInfo> GetAllUsers() => _users.Users.Values;

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
