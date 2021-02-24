using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obskurnee.Services
{
    public class RecommendationService
    {
        private readonly ILogger<RecommendationService> _logger;
        private readonly Database _db;

        public RecommendationService(
            ILogger<RecommendationService> logger,
            Database database)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IList<Post> GetAllRecs() => _db.RecPosts.FindAll().ToList();

        public IList<Post> GetRecs(string userId) => _db.RecPosts.Query().Where(p => p.OwnerId == userId).ToList();

        public Post AddRec(Post rec, string userId)
        {
            var thread = _db.RecThreads.Find(rt => rt.OwnerId == userId).FirstOrDefault();
            if (thread == null)
            {
                thread = new Discussion(userId)
                {
                    Topic = Topic.Recommendations,
                };
                _db.RecThreads.Insert(thread);
            }
            rec.PostId = 0;
            rec.DiscussionId = thread.DiscussionId;
            rec.OwnerId = userId;
            _db.RecPosts.Insert(rec);
            return rec;
        }
    }
}
