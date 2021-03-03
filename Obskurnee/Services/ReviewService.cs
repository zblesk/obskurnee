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

        public ReviewService(
            ILogger<ReviewService> logger,
            Database db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IList<Review> GetCurrentlyReading(string userId) => _db.CurrentlyReadings.Find(r => r.OwnerId == userId).ToList();
    }
}
