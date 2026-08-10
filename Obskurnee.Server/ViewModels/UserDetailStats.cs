using System.Collections.Generic;

namespace Obskurnee.ViewModels;

public class UserReadingStats
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// Books the club selected that the user has not rated/reviewed yet.
    /// </summary>
    public List<UserStatsPostItem> UnreadBooks { get; set; } = new();

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
