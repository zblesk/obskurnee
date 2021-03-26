using LiteDB;
using System.Collections.Generic;

namespace Obskurnee.Models
{
    public class Book : HeaderData
    {
        public int BookId { get; set; }
        [BsonRef("posts")] public Post Post { get; set; }
        [BsonRef("rounds")] public Round Round { get; set; }
        public int Order { get; set; }
        public int BookDiscussionId { get; set; }
        public int BookPollId { get; set; }

        public Book(string ownerId) : base(ownerId) { }
    }
}
