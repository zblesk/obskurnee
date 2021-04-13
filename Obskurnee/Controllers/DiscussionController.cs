using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
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
        public async Task<IEnumerable<Discussion>> GetDiscussions() => await _discussions.GetAll();

        [HttpGet]
        [Route("{discussionId:int}")]
        public async Task<Discussion> GetPosts(int discussionId) => await _discussions.GetWithPosts(discussionId);

        [HttpPost]
        [Route("{discussionId:int}")]
        public async Task<Post> NewPost(int discussionId, Post post)
            => await _discussions.NewPost(discussionId, post.SetOwner(User));
    }
}
