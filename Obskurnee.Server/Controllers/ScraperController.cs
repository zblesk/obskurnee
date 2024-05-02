using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

[Authorize(Policy = "CanUpdate")]
[ApiController]
[Route("api/scrape")]
public class ScraperController(ILogger<ScraperController> logger, GoodreadsScraper scraper) : Controller
{
    private readonly ILogger<ScraperController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly GoodreadsScraper _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));

    [HttpGet]
    public async Task<GoodreadsBookInfo> ScrapeUrl(string goodreadsUrl)
    {
        return await _scraper.ScrapeBookInfo(goodreadsUrl);
    }
}
