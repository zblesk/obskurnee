using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/discussions")]
public class DiscussionController : Controller
{
    private readonly DiscussionService _discussions;
    private readonly IAuthorizationService _authService;

    public DiscussionController(
        DiscussionService discussions,
        IAuthorizationService authService)
    {
        _discussions = discussions ?? throw new ArgumentNullException(nameof(discussions));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpGet]
    public Task<List<Discussion>> GetDiscussions() => _discussions.GetAll();

    [HttpGet]
    [Route("{discussionId:int}")]
    public Task<Discussion> GetPosts(int discussionId) => _discussions.GetWithPosts(discussionId);

    [HttpPost]
    [Route("{discussionId:int}")]
    [Authorize(Policy = "CanUpdate")]
    public Task<Post> NewPost(int discussionId, Post post)
        => _discussions.NewPost(discussionId, post.SetOwner(User));

    [HttpPatch]
    [Route("{discussionId:int}")]
    [Authorize(Policy = "CanUpdate")]
    public async Task<IActionResult> UpdatePost(int discussionId, Post post)
    {
        var existingPost = await _discussions.GetPost(post.PostId);
        if (existingPost == null)
            return new NotFoundResult();

        var auth = await _authService.AuthorizeAsync(User, existingPost, "EditAuthPolicy");
        if (auth.Succeeded)
            return Json(await _discussions.UpdatePost(discussionId, post.SetOwner(User)));
        return new ForbidResult();
    }
}
