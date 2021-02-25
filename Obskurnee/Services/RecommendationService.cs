using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obskurnee.Services
{
    public class RecommendationService
    {
        private readonly ILogger<RecommendationService> _logger;
        private readonly Database _db;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly NewsletterService _newsletter;

        public RecommendationService(
            ILogger<RecommendationService> logger,
            Database database,
            IStringLocalizer<NewsletterStrings> newsletterLocalizer,
            NewsletterService newsletter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
        }

        public IList<Post> GetAllRecs() => _db.RecPosts.FindAll().ToList();

        public IList<Post> GetRecs(string userId) => _db.RecPosts.Query().Where(p => p.OwnerId == userId).ToList();

        public Post AddRec(Post rec, string userId)
        {
            var thread = _db.RecThreads.Find(rt => rt.OwnerId == userId).FirstOrDefault();
            if (thread == null)
            {
                thread = new Discussion(userId)
                {
                    Topic = Topic.Recommendations,
                };
                _db.RecThreads.Insert(thread);
            }
            rec.PostId = 0;
            rec.DiscussionId = thread.DiscussionId;
            rec.OwnerId = userId;
            _db.RecPosts.Insert(rec);

            SendNewRecNotification(rec);
            return rec;
        }

        private void SendNewRecNotification(Post rec)
        {
            var link = $"{Startup.BaseUrl}/odporucania/";
            _newsletter.SendNewsletter(
                Newsletters.AllEvents,
                _newsletterLocalizer.Format("newRecSubject", rec.OwnerName),
                _newsletterLocalizer.FormatAndRender("newRecBodyMarkdown",
                    rec.Title,
                    rec.Author,
                    rec.Text.AddMarkdownQuote(),
                    rec.PageCount,
                    rec.Url,
                    link));
        }
    }
}
