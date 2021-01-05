using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class GoodreadsBookInfo : HeaderData
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int? Pages { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
