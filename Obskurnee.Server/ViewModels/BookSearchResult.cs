using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Server.ViewModels;

public record BookSearchResult(
    int? PostId,
    int? RecommendationId,
    int? DiscussionId,
    string? Title,
    string? Author,
    string? Text,
    int? PageCount,
    string? Url,
    string? ImageUrl,
    int? ParentPostId,
    int? ParentRecommendationId,
    string OwnerId,
    string OwnerName,
    DateTime CreatedOn,
    DateTime ModifiedOn,
    string Kind,
    decimal Rank,
    bool HasParent = false)
{
    public string? RenderedText { get => Text?.RenderMarkdown(); }
}

