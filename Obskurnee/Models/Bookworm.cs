using AspNetCore.Identity.LiteDB.Models;
using LiteDB;
using System.Text.RegularExpressions;

namespace Obskurnee.Models
{
    public class Bookworm : ApplicationUser
    {
        public string GoodreadsProfileUrl { get; set; }
        public string GoodreadsUserId 
        {
            get => string.IsNullOrWhiteSpace(GoodreadsProfileUrl)
                ? ""
                : Regex.Match(GoodreadsProfileUrl, @"show/(\d+)").Groups[1].Value;
        }
        public string AboutMe { get; set; }
        [BsonIgnore] public string RenderedAboutMe { get => AboutMe.RenderMarkdown(); }
    }
}
