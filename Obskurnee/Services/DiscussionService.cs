using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Obskurnee.Services
{
    public class DiscussionService
    {
        private readonly ILogger<DiscussionService> _logger;
        private readonly Database _db;
        private readonly UserService _users;
        private readonly BookService _bookService;

        public DiscussionService(
            ILogger<DiscussionService> logger,
            Database database,
            BookService bookService,
            UserService users)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }


        public IEnumerable<Discussion> GetAll()
        {
            return (from discussion in _db.Discussions.Query()
                    orderby discussion.CreatedOn descending
                    select discussion)
                    .ToList();
        }

        public Post NewPost(int discussionId, Post post)
        {
            if (_db.Discussions.FindById(discussionId).IsClosed)
            {
                throw new Exception("Diskusia uz bola uzavreta!");
            }
            post.PostId = 0; //ensure it wasn't sent from the client
            post.DiscussionId = discussionId;
            _db.Posts.Insert(post);
            return post;
        }

        public DiscussionPosts GetPosts(int discussionId)
        {
            var disc = _db.Discussions.FindById(discussionId);
            var posts = _db.Posts.Find(p => p.DiscussionId == discussionId).OrderBy(p => p.CreatedOn).ToList();
            return new(disc, posts);
        }

    }
}
