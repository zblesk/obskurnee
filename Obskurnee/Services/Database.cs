using LiteDB;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class Database
    {
        private readonly ILogger<Database> _logger;
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<Discussion> _discussions;
        private readonly ILiteCollection<Post> _posts;
        private readonly ILiteCollection<Book> _books;

        public Database(ILogger<Database> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(@"data\bookclub.db");
            _discussions = _db.GetCollection<Discussion>();
            _posts = _db.GetCollection<Post>();
            _books = _db.GetCollection<Book>();

            _posts.EnsureIndex(p => p.DiscussionId);
        }

        public IEnumerable<Discussion> GetAllDiscussions()
        {
            return (from discussion in _discussions.Query()
                    orderby discussion.CreatedOn descending
                    select discussion)
                    .ToList();
        }

        public Discussion NewDiscussion(Discussion discussion)
        {
            _discussions.Insert(discussion);
            return discussion;
        }

        public Post NewPost(int discussionId, Post post)
        {
            post.Id = 0; //ensure it wasn't sent from the client
            post.DiscussionId = discussionId;
            _posts.Insert(post);
            return post;
        }

        public DiscussionPosts GetDiscussionPosts(int discussionId)
        {
            var disc = _discussions.FindById(discussionId);
            var posts = _posts.Find(p => p.DiscussionId == discussionId).OrderBy(p => p.CreatedOn).ToList();
            return new(disc, posts);
        }

        public GoodreadsBookInfo StoreBookInfo(GoodreadsBookInfo book)
        {
            var bookInfos = _db.GetCollection<GoodreadsBookInfo>();
            bookInfos.Insert(book);
            return book;
        }
    }
}
