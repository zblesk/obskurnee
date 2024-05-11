using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;
using Obskurnee.ViewModels;
using Obskurnee.Server.ViewModels;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/search")]
public class SearchController(ILogger<SearchController> logger, SearchService search) : Controller
{
    private readonly ILogger<SearchController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly SearchService _search = search ?? throw new ArgumentNullException(nameof(search));

    [HttpGet("poststats")]
    public Task<IEnumerable<BookPostStats>> PostStats()
        => _search.GetAllBookStats();

    [HttpGet]
    public IEnumerable<BookSearchResult> Search(string query)
        => _search.Search(query);
}
