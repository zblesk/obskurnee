using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    public class PollResults
    {
        public int WinnerPostId { get; set; }
        //todo 
        [NotMapped] public List<string> AlreadyVoted { get; set; }
        /// <summary>
        /// Post ID to count
        /// </summary>
        public List<VoteResultItem> Votes { get; set; }
    }
}
