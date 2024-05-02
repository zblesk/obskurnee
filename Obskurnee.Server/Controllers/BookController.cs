using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;

namespace Obskurnee.Controllers;

[Route("api/books")]
public class BookController(ILogger<BookController> logger, BookService books) : Controller
{
    private readonly ILogger<BookController> _logger = logger;
    private readonly BookService _books = books ?? throw new ArgumentNullException(nameof(books));

    [HttpGet]
    public Task<List<Book>> GetAllBooksOrdered() => _books.GetBooksNewestFirst();
}
