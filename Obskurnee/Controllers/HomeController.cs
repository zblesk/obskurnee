using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using Obskurnee.ViewModels;
using System.Security.Claims;

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

        public HomeController(
            ILogger<HomeController> logger, 
            SettingsService settingsService,
            BookService bookService,
            UserService userService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [HttpGet]
        public JsonResult LandingPage()
        {
            if (!User.Identity.IsAuthenticated)
            {
               return Json(new { Books = new[] { _bookService.GetLatestBook() } });
            }
            var us = _userService.GetUserByEmail(User.FindFirstValue(ClaimTypes.Email));
            return Json(new
            {
                Books = _bookService.GetBooksNewestFirst(),
                Notice = _settingsService.GetSettingValue<string>(Setting.Keys.ModNoticeboard)?.RenderMarkdown(),
             //   MyProfile = UserInfo.From(us, User),
            });
        }
    }
}
