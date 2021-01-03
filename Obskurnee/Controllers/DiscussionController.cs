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

        public DiscussionController(ILogger<DiscussionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Discussion> Get()
        {
            return Enumerable.Range(1, 5).Select(i => new Discussion(i, $"Kniha {i}", $"Navrhujeme knihu #{i}!"))
                .ToArray();
        }


        [HttpGet]
        [Route("{id:int}/posts")]
        public DiscussionPosts GetPosts(int id)
        {
            return new(
                new(id, $"Kniha {id}", $"Navrhujeme knihu #{id}!"),
                new List<Post>(Enumerable.Range(1, 7)
                    .Select(i => new Post(id * 11 * i, id, $"Toto je text prispevku {i} do diskusie #{id}"))));
        }
    }
}
