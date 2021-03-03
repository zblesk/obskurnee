using AspNetCore.Identity.LiteDB.Data;
using LiteDB;
using Obskurnee.Models;
using System;
using System.IO;

namespace Obskurnee.Services
{
    public sealed class Database : ILiteDbContext, IDisposable
    {
        private readonly Serilog.ILogger _logger;
        private readonly LiteDatabase _db;
        public readonly ILiteCollection<Discussion> Discussions;
        public readonly ILiteCollection<Post> Posts;
        public readonly ILiteCollection<Discussion> RecThreads;
        public readonly ILiteCollection<Post> RecPosts;
        public readonly ILiteCollection<Book> Books;
        public readonly ILiteCollection<Poll> Polls;
        public readonly ILiteCollection<Vote> Votes;
        public readonly ILiteCollection<Round> Rounds;
        public readonly ILiteCollection<Bookworm> Users;
        public readonly ILiteCollection<Setting> Settings;
        public readonly ILiteCollection<Review> Reviews;
        public readonly ILiteCollection<Review> CurrentlyReadings;
        public readonly ILiteCollection<NewsletterSubscription> NewsletterSubscriptions;

        LiteDatabase ILiteDbContext.LiteDatabase => _db;

        public Database(
            Serilog.ILogger logger,
            Config config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(Path.Combine(config.DataFolder, "bookclub.db"));
            _db.CheckpointSize = 1;
            Discussions = _db.GetCollection<Discussion>("discussions");
            Posts = _db.GetCollection<Post>("posts");
            RecThreads = _db.GetCollection<Discussion>("recthreads");
            RecPosts = _db.GetCollection<Post>("recposts");
            Books = _db.GetCollection<Book>("books").Include(b => b.Post).Include(b => b.Round);
            Polls = _db.GetCollection<Poll>("polls").Include(p => p.Options);
            Votes = _db.GetCollection<Vote>("votes");
            Rounds = _db.GetCollection<Round>("rounds");
            Users = _db.GetCollection<Bookworm>("users");
            Settings = _db.GetCollection<Setting>("settings");
            Reviews = _db.GetCollection<Review>("reviews");
            CurrentlyReadings = _db.GetCollection<Review>("currentlyreadings");
            NewsletterSubscriptions = _db.GetCollection<NewsletterSubscription>("newslettersubscriptions");

            Posts.EnsureIndex(p => p.DiscussionId);
            Votes.EnsureIndex(v => v.PollId);
            RecPosts.EnsureIndex(r => r.OwnerId);
            NewsletterSubscriptions.EnsureIndex(ns => ns.UserId);
            NewsletterSubscriptions.EnsureIndex(ns => ns.NewsletterName);
        }

        public GoodreadsBookInfo StoreBookInfo(GoodreadsBookInfo book)
        {
            var bookInfos = _db.GetCollection<GoodreadsBookInfo>("goodreadsbookinfo");
            bookInfos.Insert(book);
            return book;
        }

        public void Dispose()
        {
            _db?.Checkpoint();
            _db?.Dispose();
        }
    }
}
