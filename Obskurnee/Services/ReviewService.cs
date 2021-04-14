using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Obskurnee.Models.GoodreadsReview.ReviewKind;

namespace Obskurnee.Services
{
    public class ReviewService
    {
        private readonly ILogger<ReviewService> _logger;
        private readonly GoodreadsScraper _scraper;
        private readonly IStringLocalizer<NewsletterStrings> _newsletterLocalizer;
        private readonly NewsletterService _newsletter;
        private readonly ApplicationDbContext _db;
        private readonly Config _config;

        public ReviewService(
            ILogger<ReviewService> logger,
            Config config,
            GoodreadsScraper scraper,
            IStringLocalizer<NewsletterStrings> newsletterLocalizer,
            NewsletterService newsletter,
            ApplicationDbContext db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
            _newsletterLocalizer = newsletterLocalizer ?? throw new ArgumentNullException(nameof(newsletterLocalizer));
            _newsletter = newsletter ?? throw new ArgumentNullException(nameof(newsletter));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        //public IEnumerable<UserInfo> GetAllUsersWithReading(bool includeCurrentlyReading = false)
        //{
        //    var users = GetAllUsers();
        //    users.Wait();
        //    foreach (var user in users.Result)
        //    {
        //        if (includeCurrentlyReading)
        //        {
        //            user.CurrentlyReading = _reviews.GetCurrentlyReading(user.UserId);
        //        }
        //        yield return user;
        //    }
        //}

        public IList<GoodreadsReview> GetCurrentlyReading(string userId) 
            => _db.GoodreadsReviews.Where(r => r.OwnerId == userId && r.Kind == CurrentlyReading).ToList();

        public async Task FetchReviewUpdates(Bookworm user)
        {
            try
            {
                _logger.LogInformation("Updating GR review from shelf RSS for {userId}", user.Id);
                _db.GoodreadsReviews.RemoveRange(
                    _db.GoodreadsReviews
                        .Where(r => r.OwnerId == user.Id
                                    && r.Kind == CurrentlyReading));
                await _db.SaveChangesAsync();
                var reading = _scraper.GetCurrentlyReadingBooks(user).ToList();
                _logger.LogInformation("Adding {count} Currently Reading for {userId}", reading.Count, user.Id);
                await _db.GoodreadsReviews.AddRangeAsync(reading);
                var readBooks = _scraper.GetReadBooks(user).ToList();
                var ids = readBooks.Select(r => r.ReviewId);
                var existing = _db.GoodreadsReviews.Where(r => ids.Contains(r.ReviewId)).ToArray();
                _logger.LogInformation("Deleting {count} Read for {userId}", existing.Length, user.Id);
                _db.GoodreadsReviews.RemoveRange(existing);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Adding {count} Read for {userId}", readBooks.Count, user.Id);
                await _db.GoodreadsReviews.AddRangeAsync(readBooks);
                await _db.SaveChangesAsync();
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
            ushort rating,
            string reviewText,
            string reviewUrl)
        {
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

        private async Task SendNewReviewNotification(BookclubReview review)
        {
            var link = $"{_config.BaseUrl}/knihy/{review.BookId}";
            await _newsletter.SendNewsletter(
                Newsletters.AllEvents,
                _newsletterLocalizer.Format("newReviewSubject", review.Book.Post.Title),
                _newsletterLocalizer.FormatAndRender("newReviewBodyMarkdown",
                    link,
                    Enumerable.Range(0, review.Rating).Aggregate("", (acc, _) => $"{acc}⭐"),
                    review.Rating,
                    review.ReviewText,
                    review.OwnerName));
        }
    }
}
