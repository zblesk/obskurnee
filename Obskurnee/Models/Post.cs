using LiteDB;

namespace Obskurnee.Models
{
    public class Post : HeaderData
    {
        public record OriginalPostReference(Topic topic, int entityId);

        public int PostId { get; set; }
        public int DiscussionId { get; set; }
        public OriginalPostReference OriginalPost { get; set; } = null;
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int PageCount { get; set; }
        public string Url { get; set; }
        [BsonIgnore] public string RenderedText { get => Text.RenderMarkdown(); }
        public string ImageUrl { get; set; }

        public Post(string ownerId) : base(ownerId) { }
    }
}
