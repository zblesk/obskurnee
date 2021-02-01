using LiteDB;

namespace Obskurnee.Models
{
    public class Review : HeaderData
    {
        public int ReviewId { get; set; }
        public ushort Rating { get; set; }
        public string ReviewText { get; set; }
        [BsonIgnore] public string RenderedReviewText { get => ReviewText.RenderMarkdown(); }

        public Review(string ownerId) : base(ownerId) { }
    }
}
