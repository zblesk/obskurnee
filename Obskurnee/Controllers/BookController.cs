using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Obskurnee.Controllers
{
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
        public JsonResult GetAllBooksOrdered() => Json(_books.GetBooksNewestFirst());
    }
}
