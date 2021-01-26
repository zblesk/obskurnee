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

        //[HttpGet]
        //[Authorize(Policy = "ModOnly")]
        //[Route("{discussionId:int}/close-voting")]
        //public Poll CloseVoting(int discussionId) => 
        //    _database.CloseDiscussionAndOpenPoll(discussionId, User.GetUserId());

        [HttpGet]
        [Route("{discussionId:int}/posts")]
        public DiscussionPosts GetPosts(int discussionId) => _discussions.GetPosts(discussionId);

        [HttpPost]
        [Route("{discussionId:int}/posts")]
        public Post NewPost(int discussionId, Post post)
        {
            return _discussions.NewPost(discussionId, post.SetOwner(User));
        }
    }
}
