using System.Collections.Generic;
using Obskurnee.Models;

namespace Obskurnee.ViewModels;

public class UserReadingStats
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// The user's average rating across all books they have rated. 0 if they haven't rated anything.
    /// </summary>
    public double AverageRating { get; set; }

    /// <summary>
    /// Number of books the user has read (i.e. rated/reviewed).
    /// </summary>
    public int BooksReadCount { get; set; }

    /// <summary>
    /// Number of recommendations the user has made.
    /// </summary>
    public int BooksRecommendedCount { get; set; }

    /// <summary>
    /// Number of club-selected books the user has not rated/reviewed yet.
    /// </summary>
    public int UnreadBooksCount { get; set; }

    /// <summary>
    /// Books the club selected that the user has not rated/reviewed yet.
    /// </summary>
    public List<Book> UnreadBooks { get; set; } = new();

    /// <summary>
    /// Deduplicated list of suggestions the user voted for, but which did not win their poll.
    /// </summary>
    public List<UserStatsPostItem> VotedButLostSuggestions { get; set; } = new();
}

public class UserStatsPostItem
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Url { get; set; }
}
