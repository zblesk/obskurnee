
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Obskurnee.Models;

[Table("GoodreadsReviews")]
[DebuggerDisplay("{BookTitle} - {Author}")]
public class GoodreadsReview : HeaderData
{
    public enum ReviewKind { Read, CurrentlyReading };

    [Key]
    public string ReviewId { get; set; }
    public string Title { get; set; }
    public string GoodreadsBookId { get; set; }
    public string ReviewUrl { get; set; }
    public string Author { get; set; }
    public ushort Rating { get; set; }
    public string ReviewText { get; set; }
    public string ImageUrl { get; internal set; }
    public ReviewKind Kind { get; set; }
    [NotMapped] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }

    public GoodreadsReview(string ownerId) : base(ownerId) { }

    public string GetStarRating()
        => Rating < 5
            ? new string('⭐', Rating)
            : "🌟🌟🌟🌟🌟";
}
