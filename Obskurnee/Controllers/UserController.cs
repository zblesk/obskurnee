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
            _logger = logger;
            _users = users;
        }

        [HttpGet]
        public IEnumerable<UserInfo> GetAllUsers() => _users.Users.Values;

        [HttpGet]
        [Route("{email}")]
        public UserInfo GetUser(string email) => UserInfo.From(_users.GetUserByEmail(email));
    }
}
