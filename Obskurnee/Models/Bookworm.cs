using AspNetCore.Identity.LiteDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class Bookworm : ApplicationUser
    {
        public string GoodreadsProfileUrl { get; set; }
    }
}
