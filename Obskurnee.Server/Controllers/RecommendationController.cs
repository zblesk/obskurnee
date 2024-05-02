using Microsoft.AspNetCore.Mvc;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/recommendations")]
public class RecommendationController(
    RecommendationService recommendations,
    IAuthorizationService authService) : Controller
{
    private readonly RecommendationService _recommendations = recommendations ?? throw new ArgumentNullException(nameof(recommendations));
    private readonly IAuthorizationService _authService = authService;

    [HttpGet]
    public IList<Recommendation> GetAllRecs() => _recommendations.GetAllRecs();

    [HttpGet]
    [Route("{userId}")]
    public IList<Recommendation> GetRecs(string userId) => _recommendations.GetRecs(userId);

    [HttpPost]
    [Authorize(Policy = "CanUpdate")]
    public async Task<Recommendation> AddRec([FromBody] PostPayload post)
        => await _recommendations.AddRec(post.ToRecommendation(User.GetUserId()));

    [HttpPatch]
    [Authorize(Policy = "CanUpdate")]
    public async Task<IActionResult> UpdateRec(PostPayload post)
    {
        var rec = post.ToRecommendation(User.GetUserId());
        var existingRec = await _recommendations.GetRec(rec.RecommendationId);
        if (existingRec == null)
            return new NotFoundResult();

        var auth = await _authService.AuthorizeAsync(User, existingRec, "EditAuthPolicy");
        if (auth.Succeeded)
            return Json(await _recommendations.UpdateRec(rec));
        return new ForbidResult();
    }
}
