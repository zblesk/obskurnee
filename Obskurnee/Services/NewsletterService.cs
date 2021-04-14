using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class NewsletterService
    {
        private readonly ILogger<NewsletterService> _logger;
        private readonly IMailerService _mailer;
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;
        private readonly MatrixService _matrix;

        public NewsletterService(
            ILogger<NewsletterService> logger,
            IMailerService mailer,
            MatrixService matrix,
            UserService userService,
            ApplicationDbContext db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));
        }

        public async Task<List<string>> Subscribe(string userId, string newsletterName)
        {
            _logger.LogInformation("Subscribing {userId} to {newsletter}", userId, newsletterName);
            var existing = _db.NewsletterSubscriptions
                .FirstOrDefault(ns => ns.NewsletterName == newsletterName && ns.UserId == userId);
            if (existing != null)
            {
                await _db.NewsletterSubscriptions.AddAsync(new NewsletterSubscription
                {
                    NewsletterName = newsletterName,
                    UserId = userId,
                });
                await _db.SaveChangesAsync();
            }
            return await GetSubscriptions(userId);
        }

        public Task<List<string>> Unsubscribe(string userId, string newsletterName)
        {
            _logger.LogInformation("Unsubscribing {userId} from {newsletter}", userId, newsletterName);
            var existing = _db.NewsletterSubscriptions
                .FirstOrDefault(ns => ns.NewsletterName == newsletterName && ns.UserId == userId);
            if (existing != null)
            {
                _db.NewsletterSubscriptions.Remove(existing);
            }
            return GetSubscriptions(userId);
        }

        public Dictionary<string, IEnumerable<UserInfo>> GetAllNewsletterSubscribers()
        {
            var result = new Dictionary<string, IEnumerable<UserInfo>>();
            foreach (var newsletter in _db.NewsletterSubscriptions
                                            .GroupBy(ns => ns.NewsletterName))
            {
                var names = newsletter.Select(ns => _userService.GetUserById(ns.UserId)).ToArray();
                Task.WaitAll(names);
                result[newsletter.Key] = names.Select(n => n.Result);
            }
            return result;
        }

        public async Task SendNewsletter(
            string newsletterName,
            string subject,
            string body,
            bool forwardSubjectToMatrix = true)
        {
            var subscribers = await GetSubscribers(newsletterName);
            _matrix.SendMessage(
                (forwardSubjectToMatrix 
                ? $"# {subject} \n".RenderMarkdown()
                : "")
                + body).Wait();
            if (subscribers.Count == 0)
            {
                _logger.LogInformation("Not sending {newsletter} with {subject}, because there are no subscribers.", newsletterName, subject);
                return;
            }
            _logger.LogInformation("Sending newsletter {newsletter} with subject {subject} to {count} subscribers.", newsletterName, subject, subscribers.Count);
            await _mailer.SendMail(subject, body, subscribers.ToArray());
        }

        public Task<List<string>> GetSubscriptions(string userId)
            => (from ns in _db.NewsletterSubscriptions
                where ns.UserId == userId
                select ns.NewsletterName)
                .ToListAsync();

        public Task<List<string>> GetSubscribers(string newsletterName)
            => (from ns in _db.NewsletterSubscriptions
                where ns.NewsletterName == newsletterName
                select ns.NewsletterName)
                .ToListAsync();
    }
}
