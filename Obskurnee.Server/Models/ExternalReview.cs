
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Obskurnee.Models;

[Table("GoodreadsReviews")]
[DebuggerDisplay("{Title} - {Author}")]
public class ExternalReview : HeaderData
{
    public enum ReviewKind { Read, CurrentlyReading };

    [Key]
    public string ReviewId { get; set; }
    public string Title { get; set; }
    [Column("GoodreadsBookId")]
    public string ExternalBookId { get; set; }
    public string? ReviewUrl { get; set; }
    public string? Author { get; set; }
    public string? Series { get; set; }
    public ushort? Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? ImageUrl { get; set; }
    public ReviewKind Kind { get; set; }
    public ExternalBookSystem ExternalSystem { get; set; } = ExternalBookSystem.Goodreads;

    [NotMapped] public string? RenderedReviewText { get => ReviewText?.RenderMarkdown(); }

    public ExternalReview(string ownerId) : base(ownerId) { }

    public string GetStarRating()
        => Rating < 5
            ? new string('⭐', Rating ?? 0)
            : "🌟🌟🌟🌟🌟";
}
