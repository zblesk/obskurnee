using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Book : HeaderData
    {
        public int BookId { get; set; }
        public int PostId { get; set; }
        public int Order { get; set; }
        public int ThemeDiscussionId { get; set; }
        public int BookDiscussionId { get; set; }
    }
}
