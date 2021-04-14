using LiteDB;
using Obskurnee.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Obskurnee.Services
{
    public sealed class Database :  IDisposable
    {
        private readonly LiteDatabase _db;
      //  public readonly ILiteCollection<Post> Posts;
     //   public readonly ILiteCollection<Book> Books;
      //  public readonly ILiteCollection<Poll> Polls;
      //  public readonly ILiteCollection<Vote> Votes;
        public readonly ILiteCollection<BookclubReview> BookReviews;
        //public readonly ILiteCollection<NewsletterSubscription> NewsletterSubscriptions;

        public Database(
            Config config)
        {
            _db = new LiteDatabase(Path.Combine(config.DataFolder, "bookclub.db"));
            _db.CheckpointSize = 1;
            // Discussions = _db.GetCollection<Discussion>("discussions");
    //        Posts = _db.GetCollection<Post>("posts");
     //       Books = _db.GetCollection<Book>("books").Include(b => b.Post).Include(b => b.Round);
       //     Polls = _db.GetCollection<Poll>("polls").Include(p => p.Options);
         //   Votes = _db.GetCollection<Vote>("votes");
             // Rounds = _db.GetCollection<Round>("rounds");
            //NewsletterSubscriptions = _db.GetCollection<NewsletterSubscription>("newslettersubscriptions");
            BookReviews = _db.GetCollection<BookclubReview>("bookclubreviews").Include(b => b.Book).Include(b => b.Book.Post);

    //        Posts.EnsureIndex(p => p.DiscussionId);
     //       Votes.EnsureIndex(v => v.PollId);
         //   NewsletterSubscriptions.EnsureIndex(ns => ns.UserId);
        //    NewsletterSubscriptions.EnsureIndex(ns => ns.NewsletterName);
            BookReviews.EnsureIndex(br => br.OwnerId);
        }


        public void Flush()
        {
            _db.Checkpoint();
        }

        public void Dispose()
        {
            _db?.Checkpoint();
            _db?.Dispose();
        }
    }
}
