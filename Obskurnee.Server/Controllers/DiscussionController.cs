using Microsoft.AspNetCore.Mvc;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Obskurnee.Controllers;

[Authorize]
[ApiController]
[Route("api/discussions")]
public partial class DiscussionController(
    DiscussionService discussions,
    IAuthorizationService authService) : Controller
{
    private readonly DiscussionService _discussions = discussions ?? throw new ArgumentNullException(nameof(discussions));
    private readonly IAuthorizationService _authService = authService ?? throw new ArgumentNullException(nameof(authService));

    [HttpGet]
    public Task<List<Discussion>> GetDiscussions() => _discussions.GetAll();

    [HttpGet]
    [Route("{discussionId:int}")]
    public Task<Discussion> GetPosts(int discussionId) => _discussions.GetWithPosts(discussionId);

    [HttpPost]
    [Route("{discussionId:int}")]
    [Authorize(Policy = "CanUpdate")]
    public Task<Post> NewPost(int discussionId, PostPayload post)
        => _discussions.NewPost(discussionId, post.ToPost(User.GetUserId()));

    [HttpPatch]
    [Route("{discussionId:int}")]
    [Authorize(Policy = "CanUpdate")]
    public async Task<IActionResult> UpdatePost(int discussionId, PostPayload postPayload)
    {
        var post = postPayload.ToPost(User.GetUserId());
        var existingPost = await _discussions.GetPost(post.PostId);
        if (existingPost == null)
            return new NotFoundResult();

        var auth = await _authService.AuthorizeAsync(User, existingPost, "EditAuthPolicy");
        if (auth.Succeeded)
            return Json(await _discussions.UpdatePost(discussionId, post.SetOwner(User)));
        return new ForbidResult();
    }

    /// <summary>
    /// Renders input Text as Markdown and returns HTML.
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("preview")]
    public IActionResult GenerateMarkdownPreview([FromBody] JObject payload)
        => Json(new { html = (payload["text"]?.ToString() ?? "").RenderMarkdown() });
}
