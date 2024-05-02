
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Obskurnee.Models;

[Table("Recommendations")]
[Index(nameof(OwnerId))]
public class Recommendation(string ownerId) : HeaderData(ownerId)
{
    [Key]
    public int RecommendationId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public int? PageCount { get; set; }
    public string? Url { get; set; }

    public string? ImageUrl { get; set; }

    [NotMapped]
    public string RenderedText { get => (Text ?? "").RenderMarkdown(); }

    [NotMapped]
    public string? GetGoodreadsId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                return null;
            }
            var match = Regex.Match(Url, @"goodreads.com\/book\/show\/(\d+).*");
            return match.Groups?[1]?.Value ?? null;
        }
    }
}
