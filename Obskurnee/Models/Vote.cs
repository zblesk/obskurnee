using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("Votes")]
    public class Vote : HeaderData
    {
        [Key]
        [BsonId]
        public string VoteId { get => $"{PollId}-{OwnerId}"; set { } }

        public int PollId { get; set; }
        public Poll Poll { get; set; }
        /// <summary>
        /// Contains votes - IDs of posts the user voted for
        /// </summary>
        // todo remove? 
        [NotMapped] public int[] PostIds { get; set; }

        public ICollection<Post> Posts { get; set; }

        public Vote(string ownerId) : base(ownerId) { }
    }
}
