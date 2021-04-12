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

    }
}
