using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;

namespace Obskurnee.Server.Controllers;

[Authorize]
[Route("api/stats")]
#if !DEBUG
[ResponseCache(Duration = 175)]
#endif
public class StatController : Controller
{
    private readonly ApplicationDbContext _db;

    public StatController(ApplicationDbContext database)
    {
        _db = database ?? throw new ArgumentNullException(nameof(database));
    }

    [HttpGet]
    [Route("bookclub")]
    public async Task<StatsOverview> GetBookclubStats()
    {
        return new StatsOverview
        {
            UserStats = await GetUserBookStats(),
            BookStats = await GetBookStats(),
        };
    }

    [HttpGet]
    [Route("user/{email}")]
    public async Task<ActionResult<UserReadingStats>> GetUserStats(string email)
    {
        var stats = await GetUserStatsInner(email);
        if (stats == null)
            return NotFound();

        return stats;
    }

    private async Task<UserReadingStats> GetUserStatsInner(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return null;
        }

        return new UserReadingStats
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            UnreadBooks = await GetUnreadBooks(user.Id),
            VotedButLostSuggestions = await GetVotedButLostSuggestions(user.Id),
        };
    }

    private async Task<List<Book>> GetUnreadBooks(string userId)
    {
        // Books the user has already rated/reviewed count as read.
        var ratedBookIds = await _db.BookReviews
            .Where(r => r.OwnerId == userId && r.Rating.HasValue)
            .Select(r => r.BookId)
            .Distinct()
            .ToListAsync();

        return await _db.BooksWithData
            .Where(b => !ratedBookIds.Contains(b.BookId))
            .OrderByDescending(b => b.Order)
            .ToListAsync();
    }

    private async Task<List<UserStatsPostItem>> GetVotedButLostSuggestions(string userId)
    {
        var userVotes = await _db.Votes
            .Include(v => v.Poll)
            .Where(v => v.OwnerId == userId)
            .ToListAsync();

        // Collect the posts the user voted for that were not among the winners of that poll.
        var lostPostIds = new HashSet<int>();
        foreach (var vote in userVotes)
        {
            var winningPostIds = vote.Poll?.IsClosed == true && vote.Poll.Results.Votes.Count > 0
                ? vote.Poll.FindWinningPosts()
                : new List<int>();

            foreach (var postId in vote.PostIds)
            {
                if (!winningPostIds.Contains(postId))
                {
                    lostPostIds.Add(postId);
                }
            }
        }

        if (lostPostIds.Count == 0)
        {
            return new List<UserStatsPostItem>();
        }

        var posts = await _db.Posts
            .Where(p => lostPostIds.Contains(p.PostId))
            .ToListAsync();

        return posts
            .Select(p => new UserStatsPostItem
            {
                PostId = p.PostId,
                Title = p.Title ?? $"#{p.PostId}",
                Author = p.Author ?? string.Empty,
                Url = p.Url ?? string.Empty,
            })
            .ToList();
    }

    private async Task<List<UserStats>> GetUserBookStats()
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

        return userStats;
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
