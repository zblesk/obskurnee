using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class ApplicationDbContext : IdentityDbContext<Bookworm>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<GoodreadsBookInfo> BookInfos { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<GoodreadsReview> GoodreadsReviews { get; set; }

        // --------------------------------------------- presunut nad, po skon
        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Round> Rounds { get; set; }
        //public DbSet<BookclubReview> BookReviews { get; set; }
        //public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }

    }
}
