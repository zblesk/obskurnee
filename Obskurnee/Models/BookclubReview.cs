using LiteDB;

namespace Obskurnee.Models
{
    public class BookclubReview : HeaderData
    {
        public string ReviewId { get; set; }
        [BsonRef] public Book Book { get; set; }
        public string GoodreadsBookId { get; set; }
        public string ReviewUrl { get; set; }
        public ushort Rating { get; set; }
        public string ReviewText { get; set; }
        [BsonIgnore] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }

        public BookclubReview(string ownerId) : base(ownerId) { }
    }
}
