using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Obskurnee.Services;
using Obskurnee.ViewModels;

namespace Obskurnee.Server.Controllers;

[Authorize]
[Route("api/stats")]
#if !DEBUG
[ResponseCache(Duration = 175)]
#endif
public class StatController : Controller
{
    private readonly StatService _statService;

    public StatController(StatService statService)
    {
        _statService = statService ?? throw new ArgumentNullException(nameof(statService));
    }

    [HttpGet]
    [Route("bookclub")]
    public Task<StatsOverview> GetBookclubStats()
        => _statService.GetBookclubStats();

    [HttpGet]
    [Route("user/{email}")]
    public async Task<ActionResult<UserReadingStats>> GetUserStats(string email)
    {
        var stats = await _statService.GetUserStats(email);
        if (stats == null)
            return NotFound();

        return stats;
    }
}
