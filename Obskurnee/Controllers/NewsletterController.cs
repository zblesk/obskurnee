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
        private readonly NewsletterService _newsletter;

        public NewsletterController(
           ILogger<NewsletterController> logger,
           NewsletterService newsletter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
        }

        [HttpGet]
        [Authorize(Policy = "ModOnly")]
        [Route("all")]
        public JsonResult GetAllSubscriptionInfo() => Json(_newsletter.GetAllNewsletterSubscribers());

        [HttpGet]
        public JsonResult GetSubscribtions() => Json(_newsletter.GetSubscriptions(User.GetUserId()));

        [HttpPost]
        [Route("{newsletterName}/subscribe")]
        public JsonResult Subscribe(string newsletterName) => Json(_newsletter.Subscribe(User.GetUserId(), newsletterName));

        [HttpPost]
        [Route("{newsletterName}/unsubscribe")]
        public JsonResult Unsubscribe(string newsletterName) => Json(_newsletter.Unsubscribe(User.GetUserId(), newsletterName));
    }
}
