using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class PollOption
    {
        public int PollOptionId { get; set; }
        /// <summary>
        /// Originating Post ID
        /// </summary>
        public int PostId { get; set; }
        public string Title { get; set; }
    }
}
