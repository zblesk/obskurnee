using AspNetCore.Identity.LiteDB.Models;
using LiteDB;

namespace Obskurnee.Models
{
    public class Bookworm : ApplicationUser
    {
        public string GoodreadsProfileUrl { get; set; }
        public string AboutMe { get; set; }
        [BsonIgnore] public string RenderedAboutMe { get => AboutMe.RenderMarkdown(); }
    }
}
