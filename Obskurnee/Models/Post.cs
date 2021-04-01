using LiteDB;
using System.Text.RegularExpressions;

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

        public string GetGoodreadsId 
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

        public Post(string ownerId) : base(ownerId) { }
    }
}
