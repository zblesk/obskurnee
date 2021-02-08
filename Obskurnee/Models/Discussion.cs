using LiteDB;
using System.Collections.Generic;

namespace Obskurnee.Models
{
    public class Discussion : HeaderData
    {
        public int DiscussionId { get; set; }
        public int PollId { get; set; }
        public int RoundId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
        virtual public Topic Topic{ get; set; }
        [BsonIgnore] public string RenderedDescription { get => Description.RenderMarkdown(); }
        [BsonRef("posts")] public IList<Post> Posts { get; set; } = new List<Post>();

        public Discussion(string ownerId) : base(ownerId) { }
    }
}
