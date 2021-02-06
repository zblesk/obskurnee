using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obskurnee.Services
{
    public class NewsletterService
    {
        private readonly ILogger<NewsletterService> _logger;
        private readonly IMailerService _mailer;
        private readonly Database _db;
        private readonly UserService _userService;

        public NewsletterService(
            ILogger<NewsletterService> logger,
            IMailerService mailer,
            UserService userService,
            Database db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IList<string> Subscribe(string userId, string newsletterName)
        {
            _logger.LogInformation("Subscribing {userId} to {newsletter}", userId, newsletterName);
            _db.NewsletterSubscriptions.Upsert(new NewsletterSubscription
            {
                NewsletterName = newsletterName,
                UserId = userId,
            });
            return GetSubscriptions(userId);
        }

        public IList<string> Unsubscribe(string userId, string newsletterName)
        {
            _logger.LogInformation("Unsubscribing {userId} from {newsletter}", userId, newsletterName);
            _db.NewsletterSubscriptions.Delete(new NewsletterSubscription
            {
                NewsletterName = newsletterName,
                UserId = userId,
            }.NewsletterSubscriptionId);
            return GetSubscriptions(userId);
        }

        public Dictionary<string, IEnumerable<UserInfo>> GetAllNewsletterSubscribers()
            => _db.NewsletterSubscriptions.FindAll()
                .GroupBy(ns => ns.NewsletterName)
                .ToDictionary(grouping => grouping.Key,
                                grouping => grouping.Select(async ns => await _userService.GetUserById(ns.UserId))
                                                    .Select(task => task.Result));


        public void SendNewsletter(string newsletterName, string subject, string body)
        {
            var subscribers = GetSubscribers(newsletterName);
            if (subscribers.Count == 0)
            {
                _logger.LogInformation("Not sending {newsletter} with {subject}, because there are no subscribers.", newsletterName, subject);
                return;
            }
            _logger.LogInformation("Sending newsletter {newsletter} with subject {subject} to {count} subscribers.", newsletterName, subject, subscribers.Count);
            _mailer.SendMail(subject, body, subscribers.ToArray());
        }

        public IList<string> GetSubscriptions(string userId)
            => (from ns in _db.NewsletterSubscriptions.Query()
                where ns.UserId == userId
                select ns.NewsletterName)
                .ToList();

        public IList<string> GetSubscribers(string newsletterName)
            => (from ns in _db.NewsletterSubscriptions.Query()
                where ns.NewsletterName == newsletterName
                select ns.NewsletterName)
                .ToList();
    }
}
