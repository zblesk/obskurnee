using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Controllers
{
    [ApiController]
    [Route("api/discussions")]
    public class DiscussionController : ControllerBase
    {
        private readonly ILogger<DiscussionController> _logger;
        private static MarkdownPipeline _mdPipeline;
        private readonly Database _database;

        static DiscussionController()
        {
            _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public DiscussionController(ILogger<DiscussionController> logger, Database database)
        {
            _logger = logger;
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        private string RenderMarkdown(string md) => Markdown.ToHtml(md, _mdPipeline);

        [HttpGet]
        public IEnumerable<Discussion> GetDiscussions() => _database.GetAllDiscussions();

        [HttpPost]
        public Discussion NewDiscussion(Discussion discussion) => _database.NewDiscussion(discussion);

        [HttpGet]
        [Route("{id:int}/posts")]
        public DiscussionPosts GetPosts(int id) => _database.GetDiscussionPosts(id);

        [HttpPost]
        [Route("{id:int}/posts")]
        public Post NewPost(int id, Post post)
        {
            post.RenderedText = RenderMarkdown(post.Text);
            return _database.NewPost(id, post);
        }
    }
}
