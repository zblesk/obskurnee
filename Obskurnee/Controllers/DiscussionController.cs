using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        public IEnumerable<Discussion> GetDiscussions() => _discussions.GetAll();

        [HttpGet]
        [Route("{discussionId:int}")]
        public Discussion GetPosts(int discussionId) => _discussions.GetWithPosts(discussionId);

        [HttpPost]
        [Route("{discussionId:int}")]
        public Post NewPost(int discussionId, Post post) => _discussions.NewPost(discussionId, post.SetOwner(User));
    }
}
