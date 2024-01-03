using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;

namespace Obskurnee.Services;

public class ApplicationDbContext : IdentityDbContext<Bookworm>
{
    private readonly ILoggerFactory _loggerFactory;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts, ILoggerFactory loggerFactory) : base(opts)
    {
        _loggerFactory = loggerFactory;
    }

    public DbSet<Setting> Settings { get; set; }
    public DbSet<GoodreadsBookInfo> BookInfos { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<ExternalReview> GoodreadsReviews { get; set; }
    public DbSet<Discussion> Discussions { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<BookclubReview> BookReviews { get; set; }
    public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
    public DbSet<StoredImage> Images { get; set; }

    public IIncludableQueryable<Poll, List<Post>> PollsWithData
        => Polls.Include(p => p.Options);

    public IIncludableQueryable<Book, Round> BooksWithData
        => Books.Include(b => b.Post).Include(b => b.Round);

    public IIncludableQueryable<BookclubReview, Post> BookReviewsWithData
        => BookReviews.Include(br => br.Book).ThenInclude(b => b.Post);

    public IQueryable<Bookworm> UsersExceptBots
        => Users.Where(u => !u.IsBot);

    public IQueryable<Bookworm> ActiveUsers
        => Users.Where(u => u.LoginEnabled && !u.IsBot);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        var task = SaveChangesAsync();
        task.Wait();
        return task.Result;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is HeaderData && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((HeaderData)entityEntry.Entity).ModifiedOn = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((HeaderData)entityEntry.Entity).CreatedOn = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
