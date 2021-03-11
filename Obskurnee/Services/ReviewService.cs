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

        public IList<Review> GetCurrentlyReading(string userId) => _db.CurrentlyReadings.Find(r => r.OwnerId == userId).ToList();

        public void FetchReviewUpdates(Bookworm user)
        {
            try
            {
                _logger.LogInformation("Updating GR review from shelf RSS for {userId}", user.Id);
                _db.CurrentlyReadings.DeleteMany(r => r.OwnerId == user.Id);
                _db.CurrentlyReadings.InsertBulk(_scraper.GetCurrentlyReadingBooks(user));
                foreach (var review in _scraper.GetReadBooks(user))
                {
                    _db.Reviews.Upsert(review);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update of GR profile for user {userId} failed", user.Id);
            }
        }
    }
}
