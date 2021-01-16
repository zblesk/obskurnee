using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class VoteResultItem
    {
        public int PostId { get; set; }
        public int Votes { get; set; }
        public int Percentage { get; set; }
    }
}
