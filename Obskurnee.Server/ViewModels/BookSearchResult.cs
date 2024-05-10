namespace Obskurnee.Server.ViewModels;

public class BookSearchResult()
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public int? PageCount { get; set; }
    public string? Url { get; set; }
    public string? ImageUrl { get; set; }
    public int? ParentPostId { get; set; }
    public int? ParentRecommendationId { get; set; }
    public string OwnerId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public bool HasParent { get; set; } = false;
    public string Kind { get; set; }
    public decimal Rank { get; set; }

}

