using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;

namespace Obskurnee.Controllers;

[Route("api/books")]
public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly BookService _books;

    public BookController(ILogger<BookController> logger, BookService books)
    {
        _logger = logger;
        _books = books ?? throw new ArgumentNullException(nameof(books));
    }

    [HttpGet]
    public Task<List<Book>> GetAllBooksOrdered() => _books.GetBooksNewestFirst();
}
