using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class RecommendationService
    {
        private readonly ILogger<RecommendationService> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly NewsletterService _newsletter;
        private readonly Config _config;

        public RecommendationService(
            ILogger<RecommendationService> logger,
            IStringLocalizer<NewsletterStrings> newsletterLocalizer,
            NewsletterService newsletter,
            Config config,
            ApplicationDbContext db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IList<Recommendation> GetAllRecs() 
            => _db.Recommendations
                .OrderByDescending(r => r.CreatedOn)
                .ToList();

        public IList<Recommendation> GetRecs(string userId) 
            => _db.Recommendations.Where(p => p.OwnerId == userId)
                .ToList();

        public async Task<Recommendation> AddRec(Recommendation rec, string userId)
        {
            rec.RecommendationId = 0;
            rec.OwnerId = userId;
            _db.Recommendations.Add(rec);
            await _db.SaveChangesAsync();
            await SendNewRecNotification(rec);
            return rec;
        }

        private async Task SendNewRecNotification(Recommendation rec)
        {
            var link = $"{_config.BaseUrl}/odporucania/";
            await _newsletter.SendNewsletter(
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
