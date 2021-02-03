using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class NewsletterService
    {
        private readonly ILogger<NewsletterService> logger;
        private readonly IMailerService mailer;
        private readonly Database _db;
        private readonly UserService _userService;

        public NewsletterService(
            ILogger<NewsletterService> logger,
            IMailerService mailer,
            UserService userService,
            Database db)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            this._db = db ?? throw new ArgumentNullException(nameof(db));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public void Subscribe(string userId, string newsletterName)
            => _db.NewsletterSubscriptions.Upsert(new NewsletterSubscription
            {
                NewsletterName = newsletterName,
                UserId = userId,
            });

        public void Unsubscribe(string userId, string newsletterName)
            => _db.NewsletterSubscriptions.Delete(new NewsletterSubscription
            {
                NewsletterName = newsletterName,
                UserId = userId,
            }.NewsletterSubscriptionId);

        public Dictionary<string, IEnumerable<UserInfo>> GetAllNewsletterSubscribers()
            => _db.NewsletterSubscriptions.FindAll()
                .GroupBy(ns => ns.NewsletterName)
                .ToDictionary(grouping => grouping.Key,
                                grouping => grouping.Select(async ns => await _userService.GetUserById(ns.UserId))
                                                    .Select(task => task.Result));


        public void SendNewsletter(string newsletterName, string subject, string body)
        {

        }

        public IList<string> GetSubscriptions(string userId)
        {
            return (from ns in _db.NewsletterSubscriptions.Query()
                    where ns.UserId == userId
                    select ns.NewsletterName)
                   .ToList();
        }

        public IList<string> GetSubscribers(string newsletterName)
        {
            return (from ns in _db.NewsletterSubscriptions.Query()
                    where ns.NewsletterName == newsletterName
                    select ns.NewsletterName)
                   .ToList();
        }
    }
}
