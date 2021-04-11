using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/scrape")]
    public class ScraperController : Controller
    {
        private readonly ILogger<ScraperController> _logger;
        private readonly GoodreadsScraper _scraper;

        public ScraperController(ILogger<ScraperController> logger, GoodreadsScraper scraper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
        }

        [HttpGet]
        public async Task<GoodreadsBookInfo> ScrapeUrl(string goodreadsUrl)
        {
            return await _scraper.ScrapeBookInfo(goodreadsUrl);
        }
    }
}
