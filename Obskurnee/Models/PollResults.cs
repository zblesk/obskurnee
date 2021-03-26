using System.Collections.Generic;

namespace Obskurnee.Models
{
    public class PollResults
    {
        public int WinnerPostId { get; set; }
        public List<string> AlreadyVoted { get; set; }
        /// <summary>
        /// Post ID to count
        /// </summary>
        public List<VoteResultItem> Votes { get; set; }
    }
}
