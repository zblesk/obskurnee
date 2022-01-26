using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/recommendations")]
public class RecommendationController : Controller
{
    private readonly ILogger<RecommendationController> _logger;
    private readonly RecommendationService _recommendations;

    public RecommendationController(
        ILogger<RecommendationController> logger,
        RecommendationService recommendations)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _recommendations = recommendations ?? throw new ArgumentNullException(nameof(recommendations));
    }

    [HttpGet]
    public IList<Recommendation> GetAllRecs() => _recommendations.GetAllRecs();

    [HttpGet]
    [Route("{userId}")]
    public IList<Recommendation> GetRecs(string userId) => _recommendations.GetRecs(userId);

    [HttpPost]
    [Authorize(Policy = "CanUpdate")]
    public async Task<Recommendation> AddRec([FromBody] Recommendation post)
        => await _recommendations.AddRec(post, User.GetUserId());
}
