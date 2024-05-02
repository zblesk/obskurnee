using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System.Diagnostics;

namespace Obskurnee.Services;

public class BookService(
    ILogger<BookService> logger,
    ApplicationDbContext database)
{
    private readonly ILogger<BookService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _db = database ?? throw new ArgumentNullException(nameof(database));

    public Task<List<Book>> GetBooksNewestFirst()
        => _db.BooksWithData.OrderByDescending(b => b.Order).ToListAsync();

    public Task<Book> GetBook(int bookId) => _db.Books.FirstAsync(b => b.BookId == bookId);

    public Task<Book> GetLatestBook() => _db.BooksWithData
        .OrderByDescending(b => b.Order)
        .FirstOrDefaultAsync();

    public async Task<Book> CreateBook(Poll poll, int roundId, string ownerId)
    {
        _logger.LogInformation("Creating book for poll {pollId}", poll.PollId);
        Trace.Assert(poll.IsClosed);
        var previousBookNo = !_db.Books.Any()
            ? 0
            : _db.Books.Max(b => b.Order);
        var book = new Book(ownerId)
        {
            BookDiscussionId = poll.DiscussionId,
            BookPollId = poll.PollId,
            Order = previousBookNo + 1,
            RoundId = roundId,
            PostId = poll.Results.WinnerPostId,
        };
        await _db.SaveChangesAsync();

        await _db.Books.AddAsync(book);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Book #{bookNo} created - ID {bookId}",
            book.Order,
            book.BookId);
        return book;
    }
}
