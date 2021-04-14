using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public Task<GroupedNewsletterSubscribers> GetAllSubscriptionInfo() 
            => _newsletter.GetAllNewsletterSubscribers();

        [HttpGet]
        public Task<List<string>> GetSubscribtions() 
            => _newsletter.GetSubscriptions(User.GetUserId());

        [HttpPost]
        [Route("{newsletterName}/subscribe")]
        public Task<List<string>> Subscribe(string newsletterName) 
            => _newsletter.Subscribe(User.GetUserId(), newsletterName);

        [HttpPost]
        [Route("{newsletterName}/unsubscribe")]
        public Task<List<string>> Unsubscribe(string newsletterName) 
            => _newsletter.Unsubscribe(
                User.GetUserId(),
                newsletterName);
    }
}
