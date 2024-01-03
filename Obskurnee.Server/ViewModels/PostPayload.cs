using Obskurnee.Models;

namespace Obskurnee.Controllers;

/// <summary>
/// For API calls
/// </summary>
public class PostPayload
{
    public int? PostId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public int? PageCount { get; set; }
    public string? Url { get; set; }

    public string? ImageUrl { get; set; }

    public int? ParentPostId { get; set; }
    public int? ParentRecommendationId { get; set; }

    internal Post ToPost(string owner) => new Post(owner)
    {
        PostId = PostId ?? 0,
        Title = Title,
        Author = Author,
        Text = Text,
        PageCount = PageCount,
        Url = Url,
        ImageUrl = ImageUrl,
        ParentPostId = ParentPostId,
        ParentRecommendationId = ParentRecommendationId
    };

    internal Recommendation ToRecommendation(string owner) => new Recommendation(owner)
    {
        RecommendationId = PostId ?? 0,
        Title = Title,
        Author = Author,
        Text = Text,
        PageCount = PageCount,
        Url = Url,
        ImageUrl = ImageUrl,
    };
}
