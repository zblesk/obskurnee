using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Obskurnee.Models;

[Table("Posts")]
[Index(nameof(DiscussionId))]
public class Post : HeaderData
{
    [Key]
    public int PostId { get; set; }

    public int DiscussionId { get; set; }
    public Discussion Discussion { get; set; }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public int? PageCount { get; set; }
    public string? Url { get; set; }

    public string? ImageUrl { get; set; }

    public int? ParentPostId { get; set; }
    public int? ParentRecommendationId { get; set; }

    [NotMapped]
    public string? RenderedText { get => Text?.RenderMarkdown(); }

    [JsonIgnore]
    public ICollection<Vote> Votes { get; set; }

    [JsonIgnore]
    public ICollection<Poll> AllPolls { get; set; }

    public string GetGoodreadsId()
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            return null;
        }
        var match = Regex.Match(Url, @"goodreads.com\/book\/show\/(\d+).*");
        return match.Groups?[1]?.Value ?? null;
    }

    public Post(string ownerId) : base(ownerId)
    {
    }
}
