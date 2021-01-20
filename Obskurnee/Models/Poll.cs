using LiteDB;
using System.Collections.Generic;

namespace Obskurnee.Models
{
    public class Poll : HeaderData
    {
        [BsonId] public int PollId { get; set; }
        public int DiscussionId { get; set; }
        public int BookId { get; set; }
        public int RoundId { get; set; }
        [BsonRef("posts")] public IList<Post> Options { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
        public bool CreateBookOnClose { get; set; } = true;
        public PollResults Results { get; set; }
        public Poll(string ownerId) : base(ownerId) { }
    }
}
