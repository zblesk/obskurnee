using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obskurnee.Services;
using Obskurnee.ViewModels;

namespace Obskurnee.Server.Controllers;

[Route("api/stats")]
[ResponseCache(Duration = 175)]
public class StatController : Controller
{
    private readonly ApplicationDbContext _db;

    public StatController(ApplicationDbContext database)
    {
        _db = database ?? throw new ArgumentNullException(nameof(database));
    }

    [HttpGet]
    [Route("users")]
    public async Task<StatsOverview> GetUserStats()
    {
        var users = await _db.ActiveUsers.ToListAsync();
        var userStats = new List<UserStats>();

        foreach (var user in users)
        {
            // Count how many books the user has rated
            var booksRatedCount = await _db.BookReviews
                .Where(r => r.OwnerId == user.Id && r.Rating.HasValue)
                .CountAsync();

            // ... how many winning books were the user's suggestion
            var winningBooksCount = await _db.Books
                .Where(b => b.Post != null && b.Post.OwnerId == user.Id)
                .CountAsync();

            // ... how many recommendations the user has made
            var recommendationCount = await _db.Recommendations
                .Where(r => r.OwnerId == user.Id)
                .CountAsync();

            userStats.Add(new UserStats
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                BooksRatedCount = booksRatedCount,
                WinningBooksCount = winningBooksCount,
                RecommendationCount = recommendationCount
            });
        }

        var bookStats = await GetBookStats();

        return new StatsOverview
        {
            UserStats = userStats,
            BookStats = bookStats,
        };
    }

    private async Task<List<BookRatingStats>> GetBookStats()
    {
        // Group the ratings per book: how many users rated it and its average rating.
        var ratingsByBook = await _db.BookReviews
            .Where(r => r.Rating.HasValue)
            .GroupBy(r => r.BookId)
            .Select(g => new
            {
                BookId = g.Key,
                RatingsCount = g.Count(),
                AverageRating = g.Average(r => (double)r.Rating.Value),
            })
            .ToListAsync();

        if (ratingsByBook.Count == 0)
        {
            return new List<BookRatingStats>();
        }

        var bookIds = ratingsByBook.Select(r => r.BookId).ToList();

        // Titles/authors live on the originating Post.
        var books = await _db.Books
            .Include(b => b.Post)
            .Where(b => bookIds.Contains(b.BookId))
            .ToDictionaryAsync(b => b.BookId);

        return ratingsByBook
            .Select(r =>
            {
                books.TryGetValue(r.BookId, out var book);
                return new BookRatingStats
                {
                    BookId = r.BookId,
                    Title = book?.Post?.Title ?? $"#{r.BookId}",
                    Author = book?.Post?.Author ?? string.Empty,
                    RatingsCount = r.RatingsCount,
                    AverageRating = r.AverageRating,
                };
            })
            .ToList();
    }
}
