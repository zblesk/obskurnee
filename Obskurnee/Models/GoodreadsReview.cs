using LiteDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("GoodreadsReviews")]
    public class GoodreadsReview : HeaderData
    {
        public enum ReviewKind { Read, CurrentlyReading };

        [Key]
        public string ReviewId { get; set; }
        public string BookTitle { get; set; }
        public string GoodreadsBookId { get; set; }
        public string ReviewUrl { get; set; }
        public string Author { get; set; }
        public ushort Rating { get; set; }
        public string ReviewText { get; set; }
        public string ImageUrl { get; internal set; }
        public ReviewKind Kind { get; set; }
        [NotMapped] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }

        public GoodreadsReview(string ownerId) : base(ownerId) { }
    }
}
