using Microsoft.AspNetCore.Mvc;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/recommendations")]
public class RecommendationController : Controller
{
    private readonly RecommendationService _recommendations;
    private readonly IAuthorizationService _authService;

    public RecommendationController(
        RecommendationService recommendations,
        IAuthorizationService authService)
    {
        _recommendations = recommendations ?? throw new ArgumentNullException(nameof(recommendations));
        _authService = authService;
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

    [HttpPatch]
    [Authorize(Policy = "CanUpdate")]
    public async Task<IActionResult> UpdateRec(Recommendation rec)
    {
        var existingRec = await _recommendations.GetRec(rec.RecommendationId);
        if (existingRec == null)
            return new NotFoundResult();

        var auth = await _authService.AuthorizeAsync(User, existingRec, "EditAuthPolicy");
        if (auth.Succeeded)
            return Json(await _recommendations.UpdateRec(rec));
        return new ForbidResult();
    }
}
