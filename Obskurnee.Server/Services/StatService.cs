using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Obskurnee.Models;
using Obskurnee.Server;
using Obskurnee.ViewModels;

namespace Obskurnee.Services;

public class StatService(
    ApplicationDbContext db,
    IMemoryCache cache)
{
    /// <summary>
    /// Gets the IDs of all posts that correspond to books that won a vote.
    /// </summary>
    /// <returns></returns>
    public HashSet<int> GetWinningPostIds() 
        => cache.GetOrCreate(
            "WinningPostIds",
            fac =>
            {
                fac.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return new HashSet<int>(
                    from b in db.Books select b.PostId ?? -1);
            });

    public async Task<StatsOverview> GetBookclubStats()
    {
        return new StatsOverview
        {
            UserStats = await GetUserBookStats(),
            BookStats = await GetBookStats(),
        };
    }

    public async Task<UserReadingStats?> GetUserStats(string email)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return null;
        }

        // Ratings the user has given (used for average rating and books-read count).
        var userRatings = await db.BookReviews
            .Where(r => r.OwnerId == user.Id && r.Rating.HasValue)
            .Select(r => (double)r.Rating.Value)
            .ToListAsync();

        var recommendationCount = await db.Recommendations
            .Where(r => r.OwnerId == user.Id)
            .CountAsync();

        var unreadBooks = await GetUnreadBooks(user.Id);

        return new UserReadingStats
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            AverageRating = userRatings.Count > 0 ? userRatings.Average() : 0,
            BooksReadCount = userRatings.Count,
            BooksRecommendedCount = recommendationCount,
            UnreadBooksCount = unreadBooks.Count,
            UnreadBooks = unreadBooks,
            VotedButLostSuggestions = await GetVotedButLostSuggestions(user.Id),
        };
    }

    private async Task<List<Book>> GetUnreadBooks(string userId)
    {
        // Books the user has already rated/reviewed count as read.
        var ratedBookIds = await db.BookReviews
            .Where(r => r.OwnerId == userId && r.Rating.HasValue)
            .Select(r => r.BookId)
            .Distinct()
            .ToListAsync();

        return await db.BooksWithData
            .Where(b => !ratedBookIds.Contains(b.BookId))
            .OrderByDescending(b => b.Order)
            .ToListAsync();
    }

    private async Task<List<Post>> GetVotedButLostSuggestions(string userId)
    {
        var userVotedPosts = await GetUsersLosingVotes(userId);

        var winningIds = GetWinningPostIds();

        userVotedPosts.ExceptWith(winningIds);

        return await db.Posts
            .Where(p => userVotedPosts.Contains(p.PostId))
            .ToListAsync();
    }

    private async Task<HashSet<int>> GetUsersLosingVotes(string userId)
        => await cache.GetOrCreateAsync(
            $"LosingVoteIds-{userId}",
            async fac =>
            {
                fac.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                return new HashSet<int>(
                (await (
                    from v in db.Votes
                    join p in db.Polls on v.PollId equals p.PollId
                    join d in db.Discussions on p.DiscussionId equals d.DiscussionId
                    where
                        v.OwnerId == userId
                        && d.Topic == Topic.Books
                        && p.IsClosed
                    orderby d.DiscussionId
                    select v.PostIds)
                    .ToListAsync())
                .SelectMany(i => i));
            });

    private async Task<List<UserStats>> GetUserBookStats()
    {
        var users = await db.ActiveUsers.ToListAsync();
        var userStats = new List<UserStats>();

        foreach (var user in users)
        {
            // Count how many books the user has rated
            var booksRatedCount = await db.BookReviews
                .Where(r => r.OwnerId == user.Id && r.Rating.HasValue)
                .CountAsync();

            // ... how many winning books were the user's suggestion
            var winningBooksCount = await db.Books
                .Where(b => b.Post != null && b.Post.OwnerId == user.Id)
                .CountAsync();

            // ... how many recommendations the user has made
            var recommendationCount = await db.Recommendations
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
        var ratingsByBook = await db.BookReviews
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
        var books = await db.Books
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
