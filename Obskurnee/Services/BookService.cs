using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class BookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserService _users;

        public BookService(
            ILogger<BookService> logger,
            ApplicationDbContext database,
            UserService users)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public Task<List<Book>> GetBooksNewestFirst()
            => _db.BooksWithData.OrderByDescending(b => b.Order).ToListAsync();

        public Task<Book> GetBook(int bookId) =>  _db.Books.FirstAsync(b => b.BookId == bookId);

        public Task<Book> GetLatestBook() => _db.Books.OrderByDescending(b => b.Order).FirstOrDefaultAsync();

        public async Task<Book> CreateBook(Poll poll, int roundId, string ownerId)
        {
            _logger.LogInformation("Creating book for poll {pollId}", poll.PollId);
            Trace.Assert(poll.IsClosed);
            var previousBookNo = _db.Books.Count() == 0
                ? 0
                : _db.Books.Max(b => b.Order);
            var book = new Book(ownerId)
            {
                BookDiscussionId = poll.DiscussionId,
                BookPollId = poll.PollId,
                Order = previousBookNo + 1,
                RoundId = roundId ,
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
}
