using LiteDB;

namespace Obskurnee.Models
{
    public class Discussion : HeaderData
    {
        public enum DiscussionKind { Other, Topics, Books };

        public int DiscussionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsArchived { get; set; }
        public DiscussionKind Kind { get; set; }
        [BsonRef("polls")] public Poll Poll { get; set; }

        public Discussion(string ownerId) : base(ownerId) { }
    }
}
