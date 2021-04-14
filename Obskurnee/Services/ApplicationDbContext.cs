using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Obskurnee.Models;
using System.Collections.Generic;

namespace Obskurnee.Services
{
    public class ApplicationDbContext : IdentityDbContext<Bookworm>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
        {
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<GoodreadsBookInfo> BookInfos { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<GoodreadsReview> GoodreadsReviews { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<BookclubReview> BookReviews { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }

        public IIncludableQueryable<Poll, List<Post>> PollsWithData
            => Polls.Include(p => p.Options);

        public IIncludableQueryable<Book, Round> BooksWithData 
            => Books.Include(b => b.Post).Include(b => b.Round);
    }
}