using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using Obskurnee.Models;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obskurnee.Services
{
    public sealed class Database : ILiteDbContext, IDisposable
    {
        private readonly object @lock = new object();
        private readonly Serilog.ILogger _logger;
        private readonly LiteDatabase _db;
        public readonly ILiteCollection<Discussion> Discussions;
        public readonly ILiteCollection<Post> Posts;
        public readonly ILiteCollection<Post> Recs;
        public readonly ILiteCollection<Book> Books;
        public readonly ILiteCollection<Poll> Polls;
        public readonly ILiteCollection<Vote> Votes;
        public readonly ILiteCollection<Round> Rounds;
        public readonly ILiteCollection<Bookworm> Users;

        LiteDatabase ILiteDbContext.LiteDatabase => _db;

        public Database(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(@"data\bookclub.db");
            _db.CheckpointSize = 1;
            Discussions = _db.GetCollection<Discussion>("discussions");
            Posts = _db.GetCollection<Post>("posts");
            Recs = _db.GetCollection<Post>("personalrecs");
            Books = _db.GetCollection<Book>("books");
            Polls = _db.GetCollection<Poll>("polls");
            Votes = _db.GetCollection<Vote>("votes");
            Rounds = _db.GetCollection<Round>("rounds");
            Users = _db.GetCollection<Bookworm>("users");

            Posts.EnsureIndex(p => p.DiscussionId);
            Votes.EnsureIndex(v => v.PollId);
            Recs.EnsureIndex(r => r.OwnerId);
        }

        public void Checkpoint()
        {
            _db.Checkpoint();
        }

        public IEnumerable<Discussion> GetAllDiscussions()
        {
            return (from discussion in Discussions.Query()
                    orderby discussion.CreatedOn descending
                    select discussion)
                    .ToList();
        }

        public Discussion NewDiscussion(Discussion discussion)
        {
            Discussions.Insert(discussion);
            return discussion;
        }

        public Post NewPost(int discussionId, Post post)
        {
            if (Discussions.FindById(discussionId).IsArchived)
            {
                throw new Exception("Diskusia uz bola uzavreta!");
            }
            post.PostId = 0; //ensure it wasn't sent from the client
            post.DiscussionId = discussionId;
            Posts.Insert(post);
            return post;
        }

        public DiscussionPosts GetDiscussionPosts(int discussionId)
        {
            var disc = Discussions.FindById(discussionId);
            var posts = Posts.Find(p => p.DiscussionId == discussionId).OrderBy(p => p.CreatedOn).ToList();
            return new(disc, posts);
        }

        public Poll CloseDiscussionAndOpenPoll(int discussionId, string currentUserId)
        {
            var discussion = Discussions.FindById(discussionId);
            if (discussion.IsArchived)
            {
                throw new PermissionException("Diskusia uz bola uzavreta!");
            }
            lock (@lock)
            {
                discussion.IsArchived = true;
                var posts = (from post in Posts.Query()
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
                Polls.Insert(poll);
                discussion.PollId = poll.PollId;
                Discussions.Update(discussion);
                return poll;
            }
        }

        public GoodreadsBookInfo StoreBookInfo(GoodreadsBookInfo book)
        {
            var bookInfos = _db.GetCollection<GoodreadsBookInfo>("goodreadsbookinfo");
            bookInfos.Insert(book);
            return book;
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
