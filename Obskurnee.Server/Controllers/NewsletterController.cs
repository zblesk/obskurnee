﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Services;
using Obskurnee.ViewModels;

namespace Obskurnee.Controllers;

[Authorize(Policy = "CanUpdate")]
[Route("api/newsletters")]
public class NewsletterController(
   ILogger<NewsletterController> logger,
   NewsletterService newsletter) : Controller
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly NewsletterService _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));

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
