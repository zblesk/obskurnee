using AspNetCore.Identity.LiteDB.Models;

namespace Obskurnee.Models
{
    public class Bookworm : ApplicationUser
    {
        public string GoodreadsProfileUrl { get; set; }
    }
}
