using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Controllers;
using Obskurnee.Models;
using Obskurnee.ViewModels;

namespace Obskurnee.Services;

public class SearchService
{
    private readonly BookService _books;
    private readonly PollService _polls;
    private readonly ApplicationDbContext _db;

    public SearchService(
        BookService books,
        PollService polls,
        ApplicationDbContext database)
    {
        _books = books ?? throw new ArgumentNullException(nameof(books));
        _polls = polls ?? throw new ArgumentNullException(nameof(polls));
        _db = database ?? throw new ArgumentNullException(nameof(database));
    }

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
}
