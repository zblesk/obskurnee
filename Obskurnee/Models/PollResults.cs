using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class PollResults
    {
        public int AlreadyVoted { get; set; }
        public int TotalVoters { get; set; }
        public bool VotingClosed { get; set; } = false;
        public List<string> YetToVote { get; set; }
        public Dictionary<int, int> Votes { get; set; }
    }
}
