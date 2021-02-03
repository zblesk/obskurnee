using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Services;
using System;

namespace Obskurnee.Controllers
{
    [Authorize]
    [Route("api/newsletters")]
    public class NewsletterController : Controller
    {
        private readonly ILogger _logger;
        private readonly SettingsService _settings;
        private readonly UserService _users;
        private readonly NewsletterService _newsletter;
        private static readonly Random _random = new Random();
        private readonly IAuthorizationService _authorizationService;

        public NewsletterController(
           ILogger<NewsletterController> logger,
           SettingsService settings,
           UserService users,
           NewsletterService newsletter,
           IAuthorizationService authorizationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        [HttpGet]
        [Authorize(Policy = "ModOnly")]
        [Route("all")]
        public JsonResult GetAllSubscriptionInfo() => Json(_newsletter.GetAllNewsletterSubscribers());

        [HttpGet]
        public JsonResult GetSubscribtions() => Json(_newsletter.GetSubscriptions(User.GetUserId()));

        [HttpPost]
        [Route("{newsletterName}/subscribe")]
        public IActionResult Subscribe(string newsletterName)
        {
            _newsletter.Subscribe(User.GetUserId(), newsletterName);
            return Ok();
        }

        [HttpPost]
        [Route("{newsletterName}/unsubscribe")]
        public IActionResult Unsubscribe(string newsletterName)
        {
            _newsletter.Unsubscribe(User.GetUserId(), newsletterName);
            return Ok();
        }

    }
}
