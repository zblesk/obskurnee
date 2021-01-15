using AspNetCore.Identity.LiteDB.Data;
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
    public class Database : ILiteDbContext, IDisposable
    {
        private object @lock = new object();
        private readonly Serilog.ILogger _logger;
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<Discussion> _discussions;
        private readonly ILiteCollection<Post> _posts;
        private readonly ILiteCollection<Book> _books;
        private readonly ILiteCollection<Poll> _polls;
        private readonly ILiteCollection<Vote> _votes;

        LiteDatabase ILiteDbContext.LiteDatabase => _db;

        public Database(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(@"data\bookclub.db");
            _db.CheckpointSize = 1;
            _discussions = _db.GetCollection<Discussion>("discussions");
            _posts = _db.GetCollection<Post>("posts");
            _books = _db.GetCollection<Book>("books");
            _polls = _db.GetCollection<Poll>("polls");
            _votes = _db.GetCollection<Vote>("votes");
            
            _posts.EnsureIndex(p => p.DiscussionId);
        }

        public void Checkpoint()
        {
            _db.Checkpoint();
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
            if (_discussions.FindById(discussionId).IsArchived)
            {
                throw new Exception("Diskusia uz bola uzavreta!");
            }
            post.PostId = 0; //ensure it wasn't sent from the client
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

        public Poll CloseDiscussionAndOpenPoll(int discussionId, string currentUserId)
        {
            var discussion = _discussions.FindById(discussionId);
            if (discussion.IsArchived)
            {
                throw new Exception("Diskusia uz bola uzavreta!");
            }
            lock (@lock)
            {
                discussion.IsArchived = true;
                var posts = (from post in _posts.Query()
                               where post.DiscussionId == discussionId
                               orderby post.PostId
                               select post)
                              .ToList();
                var poll = new Poll(currentUserId)
                {
                    DiscussionId = discussion.DiscussionId,
                    Title = discussion.Title + " - hlasovanie",
                    Options = posts,
                };
                _polls.Insert(poll);
                discussion.Poll = poll;
                _discussions.Update(discussion);
                return poll;
            }
        }

        public GoodreadsBookInfo StoreBookInfo(GoodreadsBookInfo book)
        {
            var bookInfos = _db.GetCollection<GoodreadsBookInfo>();
            bookInfos.Insert(book);
            return book;
        }

        public IEnumerable<Poll> GetAllPolls()
        {
            return (from poll in _polls.Query()
                    orderby poll.CreatedOn descending
                    select poll)
                    .ToList();
        }

        public Poll GetPoll(int pollId) => _polls
            .Include(x => x.Options)
            .FindById(pollId);

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
