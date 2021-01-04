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

        public DiscussionController(ILogger<DiscussionController> logger)
        {
            _logger = logger;
        }

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
                new List<Post>(Enumerable.Range(1, 7)
                    .Select(i => new Post(id * 11 * i, id, $"Kniha {id}", $"Toto je text prispevku {i} do diskusie #{id}"))));
            }
            return dp;
        }

        [HttpPost]
        [Route("{id:int}/posts")]
        public int NewPost(int id, Post post)
        {
            dp.Posts.Add(new Post(
                332434 + id * 11,
                id,
                post.BookTitle,
                post.Text
            ));
            return 0;
        }
    }
}
