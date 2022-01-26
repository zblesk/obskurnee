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
    private readonly ILogger<DiscussionController> _logger;
    private readonly DiscussionService _discussions;

    public DiscussionController(
        ILogger<DiscussionController> logger,
        DiscussionService discussions)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _discussions = discussions ?? throw new ArgumentNullException(nameof(discussions));
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
}
