
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Obskurnee.Models
{
    public class Bookworm : IdentityUser
    {
        public string GoodreadsProfileUrl { get; set; }
        public string GoodreadsUserId 
        {
            get => string.IsNullOrWhiteSpace(GoodreadsProfileUrl)
                ? ""
                : Regex.Match(GoodreadsProfileUrl, @"show/(\d+)").Groups[1].Value;
        }
        public string Language { get; set; }
        public string AvatarUrl { get; set; }
        public string AboutMe { get; set; }
        public string RenderedAboutMe { get => AboutMe.RenderMarkdown(); }
        public ICollection<NewsletterSubscription> NewsletterSubscriptions { get; set; }
    }
}
