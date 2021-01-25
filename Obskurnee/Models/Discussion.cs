using LiteDB;

namespace Obskurnee.Models
{
    public class Discussion : HeaderData
    {
        public int DiscussionId { get; set; }
        public int PollId { get; set; }
        public int RoundId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsArchived { get; set; }
        [BsonIgnore] public string RenderedDescription { get => Description.RenderMarkdown(); }

        public Discussion(string ownerId) : base(ownerId) { }
    }
}
