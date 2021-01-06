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
        private object @lock = new object();
        private readonly ILogger<Database> _logger;
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<Discussion> _discussions;
        private readonly ILiteCollection<Post> _posts;
        private readonly ILiteCollection<Book> _books;
        private readonly ILiteCollection<Poll> _polls;
        private readonly ILiteCollection<Vote> _votes;

        public Database(ILogger<Database> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(@"data\bookclub.db");
            _discussions = _db.GetCollection<Discussion>();
            _posts = _db.GetCollection<Post>();
            _books = _db.GetCollection<Book>();
            _polls = _db.GetCollection<Poll>();
            _votes = _db.GetCollection<Vote>();

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

        public Poll CloseDiscussionAndOpenPoll(int discussionId)
        {
            var disc = _discussions.FindById(discussionId);
            if (disc.IsArchived)
            {
                throw new Exception("Diskusia uz bola uzavreta!");
            }
            lock (@lock)
            {
                disc.IsArchived = true;
                var posts = (from post in _posts.Query()
                               where post.DiscussionId == discussionId
                               select new { post.BookTitle, post.Author, post.PostId })
                              .ToList();
                var options = posts.Select(post => new PollOption { Title = $"{post.BookTitle} - {post.Author}", PostId = post.PostId })
                                .ToList();
                var poll = new Poll
                {
                    PollId = discussionId, // let's keep them identical
                    DiscussionId = discussionId,
                    Title = disc.Title + " - hlasovanie",
                    Options = options
                };
                _polls.Insert(poll);
                disc.PollId = poll.PollId;
                _discussions.Update(disc);
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

        public Poll GetPoll(int pollId) => _polls.FindById(pollId);
    }
}
