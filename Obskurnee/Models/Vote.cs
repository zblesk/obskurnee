using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Vote : HeaderData
    {
        [BsonId]
        public string VoteId { get; set; }
        public int PollId { get; set; }
        /// <summary>
        /// Contains votes - IDs of posts the user voted for
        /// </summary>
        public int[] PostIds { get; set; }

        public Vote(string ownerId) : base(ownerId) { }
    }
}
