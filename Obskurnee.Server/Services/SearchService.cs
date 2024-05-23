using System.Text;
using Microsoft.EntityFrameworkCore;
using Obskurnee.Models;
using Obskurnee.Server.ViewModels;
using Obskurnee.ViewModels;

namespace Obskurnee.Services;

public class SearchService(
    BookService books,
    ApplicationDbContext database,
    ILogger<SearchService> logger)
{
    private readonly BookService _books = books ?? throw new ArgumentNullException(nameof(books));
    private readonly ApplicationDbContext _db = database ?? throw new ArgumentNullException(nameof(database));
    private readonly ILogger<SearchService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    const string sqliteSearchQuery = @"
with results as 
(
	select 
		rowid, 
		PostId, 
		highlight(FtsPosts, 1, '<mark class=""searchresult"">', '</mark>') Title, 
		highlight(FtsPosts, 2, '<mark class=""searchresult"">', '</mark>') Author, 
		highlight(FtsPosts, 4, '<mark class=""searchresult"">', '</mark>') Text, 
		OwnerId, 
		HasParent, 
		Kind, 
		rank
	from FtsPosts r
	where FtsPosts match {0}
	order by rank
),
resultPosts as 
(
	select 
		r.PostId, 
        null RecommendationId,
        p.DiscussionId,
		r.Title,
		r.Author, 
		r.Text, 
		p.PageCount, 
		p.Url, 
		p.ImageUrl, 
		p.ParentPostId, 
		p.ParentRecommendationId, 
		p.OwnerId, 
        usr.UserName OwnerName,
		p.CreatedOn, 
		p.ModifiedOn, 
		r.HasParent, 
		r.Kind,
		r.rank
	from results r
	join Posts p on p.PostId = r.PostId and r.Kind = 'Post'
    join AspNetUsers usr on usr.Id = p.OwnerId
),
resultRecs as 
(
	select 
		null PostId,
        r.PostId RecommendationId, 
        null DiscussionId,
		r.Title, 
		r.Author, 
		r.Text, 
		p.PageCount, 
		p.Url, 
		p.ImageUrl, 
		null ParentPostId, 
		null ParentRecommendationId, 
		p.OwnerId, 
        usr.UserName OwnerName,
		p.CreatedOn, 
		p.ModifiedOn, 
		r.HasParent, 
		r.Kind,
		r.rank
	from results r
	join Recommendations p on p.RecommendationId = r.PostId and r.Kind = 'Rec'
    join AspNetUsers usr on usr.Id = p.OwnerId
)
select *
from resultPosts
UNION
select *
from resultRecs
order by rank
limit 301";

    public async Task<IEnumerable<BookPostStats>> GetAllBookStats()
    {
        var result = new List<BookPostStats>();
        var books = await _books.GetBooksNewestFirst();
        var polls = (from poll in _db.PollsWithData
                     where poll.Topic == Topic.Books
                     select poll.Results.Votes)
                    .ToList()
                    .SelectMany(id => id);
        var postVotes = polls.GroupBy(v => v.PostId, v => v.Votes)
                            .Select(g => (PostId: g.Key, TotalVotes: g.Sum()));
        return result;
    }


    public IEnumerable<BookSearchResult> Search(string query)
    {
        _logger.LogDebug("Searching for {query}", query);
        var sanitized = SanitizeSearchQuery(query);
        _logger.LogDebug("Sanitized query: {query}", sanitized);
        var res = _db.Database.SqlQueryRaw<BookSearchResult>(
            sqliteSearchQuery,
            sanitized)
            .ToList();
        _logger.LogDebug("Found {count} matches", res.Count);
        return res;
    }

    public string SanitizeSearchQuery(string query)
    {
        var sanitized = new StringBuilder();
        var quoteAllowed = query.Count(c => c == '"') % 2 == 0;

        foreach (var ch in query)
            if (char.IsLetterOrDigit(ch)
                || ch == ' '
                || ch == '*'
                || (quoteAllowed && ch == '"'))
                sanitized.Append(ch);

        if (char.IsLetterOrDigit(sanitized[^1]))
            sanitized.Append('*');
        return sanitized.ToString();
    }
}
