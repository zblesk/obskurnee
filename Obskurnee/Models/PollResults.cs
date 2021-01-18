using System.Collections.Generic;

namespace Obskurnee.Models
{
    public class PollResults
    {
        public int WinnerPostId { get; set; }
        public int AlreadyVoted { get; set; }
        public int TotalVoters { get; set; }
        public List<string> YetToVote { get; set; }
        /// <summary>
        /// Post ID to count
        /// </summary>
        public IEnumerable<VoteResultItem> Votes { get; set; }
    }
}
