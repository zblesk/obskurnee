using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{    public class Post : HeaderData
    {
        public int PostId { get; set; }
        public int DiscussionId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int? PageCount { get; set; }
        public string Url { get; set; }
        public string RenderedText { get; set; }
        public string ImageUrl { get; set; }
    }
}
