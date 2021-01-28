using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Obskurnee.Services
{
    public class BookService
    {
        private readonly ILogger<BookService> _logger;
        private readonly Database _db;
        private readonly UserService _users;

        public BookService(
            ILogger<BookService> logger,
            Database database,
            UserService users)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public IEnumerable<Book> GetBooksNewestFirst()
        {
            return _db.Books.Include(b => b.Post).FindAll().OrderByDescending(b => b.Order);
        }

        public BookInfo GetBook(int bookId) => new BookInfo { Book = _db.Books.FindById(bookId) };

        public Book CreateBook(Poll poll, int roundId, string ownerId)
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
                Round = new Round(null) { RoundId = roundId },
                Post = new Post(null) { PostId = poll.Results.WinnerPostId },
            };
            _db.Books.Insert(book);
            _logger.LogInformation("Book #{bookNo} created - ID {bookId}, based on post {postId}. {@book}",
                book.Order,
                book.BookId,
                book.Post.PostId,
                book);
            return book;
        }
    }
}
