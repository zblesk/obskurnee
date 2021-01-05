using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Discussion : HeaderData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isArchived { get; set; }
    }

    public class Post : HeaderData
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public string BookTitle { get; set; }
        public string Text { get; set; }
        public int? PageCount { get; set; }
        public string Url { get; set; }
        public string RenderedText { get; set; }
    }
}
