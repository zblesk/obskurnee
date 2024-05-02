using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Obskurnee.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/home")]
public class HomeController(
    ILogger<HomeController> logger,
    SettingsService settingsService,
    BookService bookService,
    UserServiceBase userService,
    DiscussionService discussionService,
    PollService pollService,
    MatrixService matrixService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly SettingsService _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    private readonly BookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    private readonly UserServiceBase _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    private readonly DiscussionService _discussionService = discussionService ?? throw new ArgumentNullException(nameof(discussionService));
    private readonly PollService _pollService = pollService ?? throw new ArgumentNullException(nameof(pollService));
    private readonly MatrixService _matrixService = matrixService;

    [HttpGet]
    public async Task<JsonResult> LandingPage()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Json(new
            {
                Books = new[] { await _bookService.GetLatestBook() },
                SiteName = Config.Current.SiteName,
                DefaultLanguage = Config.Current.DefaultCulture,
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
            DefaultLanguage = Config.Current.DefaultCulture,
        });
    }
}
