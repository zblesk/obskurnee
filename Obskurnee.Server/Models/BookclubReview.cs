
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models;

[Table("BookclubReviews")]
public class BookclubReview(string ownerId) : HeaderData(ownerId)
{
    [Key] public string ReviewId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string? GoodreadsBookId { get; set; }
    public string? ReviewUrl { get; set; }
    public ushort? Rating { get; set; }
    public string? ReviewText { get; set; }
    [NotMapped] public string? RenderedReviewText { get => ReviewText?.RenderMarkdown(); }
}
