using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Book : HeaderData
    {
        public int BookId { get; set; }
        [BsonRef("posts")]
        public Post Post { get; set; }
        public int Order { get; set; }
        public int ThemeDiscussionId { get; set; }
        public int ThemePollId { get; set; }
        public int BookDiscussionId { get; set; }
        public int BookPollId { get; set; }

        public Book(string ownerId) : base(ownerId) { }
    }
}
