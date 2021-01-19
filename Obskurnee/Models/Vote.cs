﻿using LiteDB;

namespace Obskurnee.Models
{
    public class Vote : HeaderData
    {
        [BsonId]
        public string VoteId => $"{PollId}-{OwnerId}";
        public int PollId { get; set; }
        /// <summary>
        /// Contains votes - IDs of posts the user voted for
        /// </summary>
        public int[] PostIds { get; set; }

        public Vote(string ownerId) : base(ownerId) { }
    }
}