using LiteDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("BookclubReviews")]
    public class BookclubReview : HeaderData
    {
        [Key] [BsonId] public string ReviewId { get; set; }
        public int BookId { get; set; }
        [BsonRef] public Book Book { get; set; }
        public string GoodreadsBookId { get; set; }
        public string ReviewUrl { get; set; }
        public ushort Rating { get; set; }
        public string ReviewText { get; set; }
        [NotMapped] [BsonIgnore] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }

        public BookclubReview(string ownerId) : base(ownerId) { }
    }
}
