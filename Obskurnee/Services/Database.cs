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
        public readonly ILiteCollection<Setting> Settings;

        LiteDatabase ILiteDbContext.LiteDatabase => _db;

        public Database(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = new LiteDatabase(@"data\bookclub.db");
            _db.CheckpointSize = 1;
            Discussions = _db.GetCollection<Discussion>("discussions");
            Posts = _db.GetCollection<Post>("posts");
            Recs = _db.GetCollection<Post>("personalrecs");
            Books = _db.GetCollection<Book>("books").Include(b => b.Post).Include(b => b.Round);
            Polls = _db.GetCollection<Poll>("polls").Include(p => p.Options);
            Votes = _db.GetCollection<Vote>("votes");
            Rounds = _db.GetCollection<Round>("rounds");
            Users = _db.GetCollection<Bookworm>("users");
            Settings = _db.GetCollection<Setting>("settings");
            Posts.EnsureIndex(p => p.DiscussionId);
            Votes.EnsureIndex(v => v.PollId);
            Recs.EnsureIndex(r => r.OwnerId);
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
