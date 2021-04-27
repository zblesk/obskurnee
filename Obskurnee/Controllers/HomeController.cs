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
        private readonly UserServiceBase _userService;
        private readonly DiscussionService _discussionService;
        private readonly PollService _pollService;
        private readonly MatrixService _matrixService;

        public HomeController(
            ILogger<HomeController> logger, 
            SettingsService settingsService,
            BookService bookService,
            UserServiceBase userService,
            DiscussionService discussionService,
            PollService pollService,
            MatrixService matrixService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _discussionService = discussionService ?? throw new ArgumentNullException(nameof(discussionService));
            _pollService = pollService ?? throw new ArgumentNullException(nameof(pollService));
            _matrixService = matrixService;
        }

        [HttpGet]
        public async Task<JsonResult> LandingPage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    Books = new[] { await _bookService.GetLatestBook() },
                    SiteName = Config.Current.SiteName,
                });
            }
            return Json(new
            {
                Books = await _bookService.GetBooksNewestFirst(),
                Notice = _settingsService.GetSettingValue<string>(Setting.Keys.ModNoticeboard)?.RenderMarkdown(),
                MyProfile = await _userService.GetUserByEmail(User.FindFirstValue(ClaimTypes.Email)),
                SiteName = Config.Current.SiteName,
                CurrentPoll = await _pollService.GetLatestOpen(),
                CurrentDiscussion = await _discussionService.GetLatestOpen(),
                MatrixRoomLink = _matrixService?.GetEnabledMatrixRoomLink(),
            });
        }
    }
}
