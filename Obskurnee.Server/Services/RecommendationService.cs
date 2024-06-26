﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Obskurnee.Models;
using Obskurnee.Server;

namespace Obskurnee.Services;

public class RecommendationService(
    IStringLocalizer<NewsletterStrings> newsletterLocalizer,
    NewsletterService newsletter,
    Config config,
    ApplicationDbContext db)
{
    private readonly ApplicationDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
    private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
    private readonly NewsletterService _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
    private readonly Config _config = config ?? throw new ArgumentNullException(nameof(config));

    public IList<Recommendation> GetAllRecs()
        => _db.Recommendations
            .OrderByDescending(r => r.CreatedOn)
            .ToList();

    public IList<Recommendation> GetRecs(string userId)
        => _db.Recommendations.Where(p => p.OwnerId == userId)
            .ToList();

    public async Task<Recommendation> AddRec(Recommendation rec)
    {
        rec.RecommendationId = 0;
        _db.Recommendations.Add(rec);
        await _db.SaveChangesAsync();
        await SendNewRecNotification(rec);
        return rec;
    }

    /// <summary>
    /// Get the Recommendation, WITHOUT tracking it in the context
    /// </summary>
    /// <returns></returns>
    public Task<Recommendation> GetRec(int recId)
        => _db.Recommendations.AsNoTracking().FirstOrDefaultAsync(p => p.RecommendationId == recId);

    public async Task<Recommendation> UpdateRec(Recommendation rec)
    {
        var existingRec = await _db.Recommendations.SingleAsync(
            p => p.RecommendationId == rec.RecommendationId);
        existingRec.PageCount = rec.PageCount;
        existingRec.Text = rec.Text;
        existingRec.Title = rec.Title;
        existingRec.Author = rec.Author;
        _db.Recommendations.Update(existingRec);
        await _db.SaveChangesAsync();
        return existingRec;
    }

    private async Task SendNewRecNotification(Recommendation rec)
    {
        var link = $"{_config.BaseUrl}/odporucania/";
        await _newsletter.SendNewsletter(
            Newsletters.AllEvents,
            _newsletterLocalizer.Format("newRecSubject", rec.OwnerName),
            _newsletterLocalizer.Format("newRecBodyMarkdown",
                rec.Title,
                rec.Author,
                rec.Text.AddMarkdownQuote(),
                rec.PageCount,
                rec.Url,
                link));
    }
}
