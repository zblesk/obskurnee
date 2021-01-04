using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
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
        static DiscussionPosts dp = null;
        private static MarkdownPipeline _mdPipeline;

        static DiscussionController()
        {
            _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public DiscussionController(ILogger<DiscussionController> logger)
        {
            _logger = logger;
        }

        private string RenderMarkdown(string md) => Markdown.ToHtml(md, _mdPipeline);

        [HttpGet]
        public IEnumerable<Discussion> Get()
        {
            return Enumerable.Range(1, 5).Select(i => new Discussion(i, $"Kniha {i} - Navrhy", $"Navrhujeme knihu #{i}!"))
                .ToArray();
        }


        [HttpGet]
        [Route("{id:int}/posts")]
        public DiscussionPosts GetPosts(int id)
        {
            if (dp == null)
            {
                dp = new(
                new(id, $"Kniha {id} - Navrhy", $"Navrhujeme knihu #{id}!"),
                new List<Post>(Enumerable.Range(4, 3)
                    .Select(i => new Post(id * 11 * i, id, $"Kniha {id}", $"Toto je <b>text</b> prispevku {i} <br /> do diskusie #{id}"))));
            }
            return dp;
        }

        [HttpPost]
        [Route("{id:int}/posts")]
        public Post NewPost(int id, Post post)
        {
            var newPost = post with { RenderedText = RenderMarkdown(post.Text), Id = id * 117 };
            dp.Posts.Add(newPost);
            return newPost;
        }
    }
}
