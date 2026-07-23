using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obskurnee.Services;
using Obskurnee.ViewModels;

namespace Obskurnee.Server.Controllers;

[Route("api/stats")]
[ResponseCache(Duration = 1800)] // 30 min
public class StatController : Controller
{
    private readonly ApplicationDbContext _db;

    public StatController(ApplicationDbContext database)
    {
        _db = database ?? throw new ArgumentNullException(nameof(database));
    }

    [HttpGet]
    [Route("users")]
    public async Task<List<UserStats>> GetUserStats()
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
                BooksRatedCount = booksRatedCount,
                WinningBooksCount = winningBooksCount,
                RecommendationCount = recommendationCount
            });
        }

        return userStats;
    }
}
