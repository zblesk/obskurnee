﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using static Obskurnee.Models.ExternalReview.ReviewKind;
using static Obskurnee.Models.ExternalBookSystem;
using Obskurnee.Server;

namespace Obskurnee.Services;

public class ReviewService(
    ILogger<ReviewService> logger,
    Config config,
    GoodreadsScraper goodreadsScraper,
    StorygraphScraper storygraphScraper,
    IStringLocalizer<NewsletterStrings> newsletterLocalizer,
    IStringLocalizer<Strings> localizer,
    NewsletterService newsletter,
    MatrixService matrix,
    ApplicationDbContext db)
{
    private readonly ILogger<ReviewService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly GoodreadsScraper _goodreadsScraper = goodreadsScraper ?? throw new ArgumentNullException(nameof(goodreadsScraper));
    private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
    private readonly NewsletterService _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
    private readonly ApplicationDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
    private readonly Config _config = config ?? throw new ArgumentNullException(nameof(config));
    private readonly StorygraphScraper _storygraphScraper = storygraphScraper;
    private readonly IStringLocalizer<Strings> _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    private static readonly ReviewIdComparer _comparer = new();
    private readonly MatrixService _matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));

    public Task<List<ExternalReview>> GetAllCurrentlyReading()
        => _db.GoodreadsReviews.Where(r => r.Kind == CurrentlyReading).ToListAsync();

    public async Task FetchReviewUpdates(Bookworm user)
    {
        try
        {
            var newCurrent = await UpdateCurrentlyReading(user);
            var newReviews = await UpdateRead(user);
            await _db.SaveChangesAsync();

#pragma warning disable CS8604 // Possible null reference argument.
            foreach (var r in newCurrent)
            {
                await _matrix.SendMessage(
                    _newsletterLocalizer.Format("newBookCurrentlyReadingNotification",
                        user.UserName,
                        r.Title,
                        r.Author,
                        r.ReviewUrl));
            }
            foreach (var r in newReviews.Take(10))
            {
                if (!string.IsNullOrWhiteSpace(r.ReviewText))
                {
                    await _matrix.SendMessage(
                        _newsletterLocalizer.Format("newBookReadNotificationReview",
                            user.UserName,
                            r.Title,
                            r.Author,
                            r.GetStarRating(),
                            r.ReviewText.AddMarkdownQuote(),
                            r.ReviewUrl));
                }
                else
                {
                    await _matrix.SendMessage(
                        _newsletterLocalizer.Format("newBookReadNotificationNoReview",
                            user.UserName,
                            r.Title,
                            r.Author,
                            r.GetStarRating(),
                            r.ReviewUrl));
                }
            }
#pragma warning restore CS8604 // Possible null reference argument.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update of GR profile for user {userId} failed", user.Id);
        }
    }

    public Task<List<BookclubReview>> GetBookReviews(int bookId)
        => _db.BookReviewsWithData.Where(br => br.Book.BookId == bookId).ToListAsync();

    public Task<List<BookclubReview>> GetUserReviews(string userId)
        => _db.BookReviewsWithData.Where(br => br.OwnerId == userId).ToListAsync();

    public async Task<BookclubReview> UpsertBookclubBookReview(
        int bookId,
        string userId,
        ushort? rating,
        string? reviewText,
        string? reviewUrl)
    {
        if (rating > 5)
        {
            throw new ValidationException(_localizer["invalidStarRating"]);
        }
        var book = await _db.BooksWithData.FirstAsync(b => b.BookId == bookId);
        var review = new BookclubReview(userId)
        {
            ReviewId = $"{bookId}-{userId}",
            Book = book,
            ReviewText = reviewText,
            Rating = rating,
            ReviewUrl = reviewUrl,
        };
        var existing = await _db.BookReviews.FirstOrDefaultAsync(br => br.ReviewId == review.ReviewId);
        if (existing != null)
        {
            _db.BookReviews.Remove(existing);
            await _db.SaveChangesAsync();
        }
        await _db.BookReviews.AddAsync(review);
        await _db.SaveChangesAsync();
        await SendNewReviewNotification(review);
        return review;
    }

    public Task<List<ExternalReview>> GetAllGoodreadsReviewsSince(DateTime dateStart)
        => _db.GoodreadsReviews.Where(grr => grr.CreatedOn >= dateStart).ToListAsync();


    private async Task SendNewReviewNotification(BookclubReview review)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var link = $"{_config.BaseUrl}/knihy/{review.BookId}";
        await _newsletter.SendNewsletter(
            Newsletters.AllEvents,
            _newsletterLocalizer.Format("newReviewSubject",
                review.Book.Post?.Title,
                review.OwnerName),
            _newsletterLocalizer.Format("newReviewBodyMarkdown",
                link,
                Enumerable.Range(0, review.Rating ?? 0).Aggregate("", (acc, _) => $"{acc}⭐"),
                review.ReviewText?.AddMarkdownQuote(),
                review.OwnerName));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    /// Updates the user's Currently Reading shelf
    /// </summary>
    /// <returns>Returns a sequence of _newly added_ reviews.</returns>
    private async Task<IEnumerable<ExternalReview>> UpdateCurrentlyReading(Bookworm user)
    {
        _logger.LogInformation("Updating GR review from shelf RSS for {userId}", user.Id);

        var existing = _db.GoodreadsReviews
                        .Where(r => r.OwnerId == user.Id
                                    && r.Kind == CurrentlyReading
                                    && r.ExternalBookId != null)
                        .ToHashSet(_comparer);

        var scraper = GetUsersScraper(user);

        if (scraper == null)
        {
            _logger.LogDebug("No scraper identified for {username}", user.UserName);
            return Enumerable.Empty<ExternalReview>();
        }

        var currentlyReading = scraper
                        .GetCurrentlyReadingBooks(user)
                        .Where(r => !string.IsNullOrWhiteSpace(r.ExternalBookId))
                        .ToHashSet(_comparer);

        var existingCopy = new HashSet<ExternalReview>(existing, _comparer);
        existing.ExceptWith(currentlyReading);
        currentlyReading.ExceptWith(existingCopy);

        _logger.LogInformation("Removing {count} Currently Reading for {userId}", existing.Count, user.Id);
        _db.GoodreadsReviews.RemoveRange(existing);
        _logger.LogInformation("Adding {count} Currently Reading for {userId}", currentlyReading.Count, user.Id);
        await _db.GoodreadsReviews.AddRangeAsync(currentlyReading);
        return currentlyReading;
    }

    /// <summary>
    /// Updates the user's Currently Read
    /// </summary>
    /// <returns>Returns a sequence of _newly added_ reviews that at least have a rating or a review.</returns>
    private async Task<IEnumerable<ExternalReview>> UpdateRead(Bookworm user)
    {
        var scraper = GetUsersScraper(user);

        if (scraper == null)
        {
            _logger.LogDebug("No scraper identified for {username}", user.UserName);
            return Enumerable.Empty<ExternalReview>();
        }

        var readBooks = scraper
                        .GetReadBooks(user)
                        .Where(r => !string.IsNullOrWhiteSpace(r.ExternalBookId))
                        .ToHashSet(_comparer);
        var readIds = readBooks.Select(r => r.ExternalBookId);

        var existing = (from r in _db.GoodreadsReviews
                        where r.OwnerId == user.Id
                             && r.Kind == Read
                             && r.ExternalBookId != null
                             && readIds.Contains(r.ExternalBookId)
                        select r)
                       .ToHashSet(_comparer);
        readBooks.ExceptWith(existing);

        _logger.LogInformation("Adding {count} Read for {userId}", readBooks.Count, user.Id);
        await _db.GoodreadsReviews.AddRangeAsync(readBooks);
        return readBooks
                .Where(r => r.Rating > 0 || !string.IsNullOrWhiteSpace(r.ReviewText));
    }

    private IExternalBookScraper? GetUsersScraper(Bookworm user)
        => user.ExternalProfileSystem switch
        {
            Goodreads => _goodreadsScraper,
            Storygraph => _storygraphScraper,
            _ => null
        };
}
