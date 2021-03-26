using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obskurnee.Services
{
    public class ReviewService
    {
        private readonly ILogger<ReviewService> _logger;
        private readonly Database _db;
        private readonly GoodreadsScraper _scraper;

        public ReviewService(
            ILogger<ReviewService> logger,
           GoodreadsScraper scraper,
            Database db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
        }

        public IList<GoodreadsReview> GetCurrentlyReading(string userId) => _db.CurrentlyReadings.Find(r => r.OwnerId == userId).ToList();

        public void FetchReviewUpdates(Bookworm user)
        {
            try
            {
                _logger.LogInformation("Updating GR review from shelf RSS for {userId}", user.Id);
                _db.CurrentlyReadings.DeleteMany(r => r.OwnerId == user.Id);
                _db.CurrentlyReadings.InsertBulk(_scraper.GetCurrentlyReadingBooks(user));
                foreach (var review in _scraper.GetReadBooks(user))
                {
                    _db.GoodreadsReviews.Upsert(review);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update of GR profile for user {userId} failed", user.Id);
            }
        }

        public IEnumerable<BookclubReview> GetBookReviews(int bookId) => _db.BookReviews.Find(br => br.Book.BookId == bookId);

        public IEnumerable<BookclubReview> GetUserReviews(string userId) => _db.BookReviews.Find(br => br.OwnerId == userId);

        public BookclubReview UpsertBookclubBookReview(
            int bookId,
            string userId,
            ushort rating,
            string reviewText,
            string reviewUrl)
        {
            var book = _db.Books.FindById(bookId);
            var review = new BookclubReview(userId)
            {
                ReviewId = $"{bookId}-{userId}",
                Book = book,
                ReviewText = reviewText,
                Rating = rating,
                ReviewUrl = reviewUrl,
            };
            _db.BookReviews.Upsert(review);
            return review;
        }
    }
}
