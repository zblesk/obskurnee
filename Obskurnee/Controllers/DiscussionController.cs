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
        private readonly Database _database;

        public DiscussionController(ILogger<DiscussionController> logger, Database database, UserManager<Bookworm> userManager)
        {
            _logger = logger;
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        public IEnumerable<Discussion> GetDiscussions() => _database.GetAllDiscussions();

        [HttpPost]
        public Discussion NewDiscussion(Discussion discussion) => _database.NewDiscussion(discussion.SetOwner(User));

        [HttpGet]
        [Authorize(Policy = "ModOnly")]
        [Route("{discussionId:int}/close-voting")]
        public Poll CloseVoting(int discussionId) => 
            _database.CloseDiscussionAndOpenPoll(discussionId, User.GetUserId());

        [HttpGet]
        [Route("{discussionId:int}/posts")]
        public DiscussionPosts GetPosts(int discussionId) => _database.GetDiscussionPosts(discussionId);

        [HttpPost]
        [Route("{discussionId:int}/posts")]
        public Post NewPost(int discussionId, Post post)
        {
            // post.RenderedText = post.Text; //  RenderMarkdown(post.Text);
            return _database.NewPost(discussionId, post.SetOwner(User));
        }
    }
}
