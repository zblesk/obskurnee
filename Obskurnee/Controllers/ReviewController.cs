using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Obskurnee.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Obskurnee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : Controller
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly ReviewService _reviews;

        public ReviewController(
            ILogger<ReviewController> logger,
            ReviewService reviews)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviews = reviews ?? throw new ArgumentNullException(nameof(reviews));
        }

        [HttpGet]
        [Route("book/{bookId:int}")]
        public IEnumerable<BookclubReview> GetForBook(int bookId) => _reviews.GetBookReviews(bookId);
        
        [HttpGet]
        [Route("book/{userId:string}")]
        public IEnumerable<BookclubReview> GetForUser(string userId) => _reviews.GetUserReviews(userId);

        [HttpPost]
        [Route("book/{bookId:int}")]
        public BookclubReview UpsertReview(int bookId, [FromBody] BookclubReview reviewData)
            => _reviews.UpsertBookclubBookReview(bookId, User.GetUserId(), reviewData.Rating, reviewData.ReviewText, reviewData.ReviewUrl);
    }
}
