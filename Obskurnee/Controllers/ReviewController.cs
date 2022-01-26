using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using Obskurnee.Services;
using Microsoft.AspNetCore.Authorization;

namespace Obskurnee.Controllers;

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
    public Task<List<BookclubReview>> GetForBook(int bookId)
        => _reviews.GetBookReviews(bookId);

    [HttpGet]
    [Route("user/{userId}")]
    public Task<List<BookclubReview>> GetForUser(string userId)
        => _reviews.GetUserReviews(userId);

    [HttpPost]
    [Route("book/{bookId:int}")]
    [Authorize(Policy = "CanUpdate")]
    public Task<BookclubReview> UpsertReview(int bookId, [FromBody] BookclubReview reviewData)
        => _reviews.UpsertBookclubBookReview(
            bookId,
            User.GetUserId(),
            reviewData.Rating,
            reviewData.ReviewText,
            reviewData.ReviewUrl);

    [HttpGet]
    [Route("currentlyreading")]
    public Task<List<GoodreadsReview>> GetCurrentlyReadings()
        => _reviews.GetAllCurrentlyReading();

}
