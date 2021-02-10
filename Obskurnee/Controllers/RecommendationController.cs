using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationController : Controller
    {
        private readonly ILogger<RecommendationController> _logger;
        private readonly RecommendationService _recommendations;

        public RecommendationController(
            ILogger<RecommendationController> logger,
            RecommendationService recommendations)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recommendations = recommendations ?? throw new ArgumentNullException(nameof(recommendations));
        }

        [HttpGet]
        public IList<Post> GetAllRecs() => _recommendations.GetAllRecs();

        [HttpGet]
        [Route("{userId}")]
        public IList<Post> GetRecs(string userId) => _recommendations.GetRecs(userId);

        [HttpPost]
        public Post AddRec()
        {
            // auth? 
            // param with target book? 
            return _recommendations.AddRec(null, User.GetUserId());
        }
    }
}
