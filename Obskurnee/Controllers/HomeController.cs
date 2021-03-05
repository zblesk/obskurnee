using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SettingsService _settingsService;
        private readonly BookService _bookService;
        private readonly UserService _userService;
        private readonly DiscussionService _discussionService;
        private readonly PollService _pollService;

        public HomeController(
            ILogger<HomeController> logger, 
            SettingsService settingsService,
            BookService bookService,
            UserService userService,
            DiscussionService discussionService,
            PollService pollService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _discussionService = discussionService ?? throw new ArgumentNullException(nameof(discussionService));
            _pollService = pollService ?? throw new ArgumentNullException(nameof(pollService));
        }

        [HttpGet]
        public async Task<JsonResult> LandingPage()
        {
            if (!User.Identity.IsAuthenticated)
            {
               return Json(new { Books = new[] { _bookService.GetLatestBook() } });
            }
            return Json(new
            {
                Books = _bookService.GetBooksNewestFirst(),
                Notice = _settingsService.GetSettingValue<string>(Setting.Keys.ModNoticeboard)?.RenderMarkdown(),
                MyProfile = await _userService.GetUserByEmail(User.FindFirstValue(ClaimTypes.Email)),
                SiteName = "",
                CurrentPoll = _pollService.GetLatestOpen(),
                CurrentDiscussion = _discussionService.GetLatestOpen(),
            });
        }
    }
}
