using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Poll : HeaderData
    {
        public int PollId { get; set; }
        /// <summary>
        /// Discussion ID. Let's keep it identical to Poll ID for now, by convention.
        /// </summary>
        public int DiscussionId { get; set; }
        public IList<PollOption> Options { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
    }
}
