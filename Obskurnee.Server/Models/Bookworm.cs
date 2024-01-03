
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using static Obskurnee.Models.ExternalBookSystem;

namespace Obskurnee.Models;

public class Bookworm : IdentityUser
{
    private string _externalProfileUrl = null;

    [Column("GoodreadsProfileUrl")]
    public string ExternalProfileUrl
    {
        get => _externalProfileUrl;
        set
        {
            _externalProfileUrl = value;
            if (_externalProfileUrl.StartsWith("https://www.goodreads.com"))
                ExternalProfileSystem = Goodreads;
            else if (_externalProfileUrl.StartsWith("https://app.thestorygraph.com/"))
                ExternalProfileSystem = Storygraph;
            else ExternalProfileSystem = null;
        }
    }
    public string ExternalProfileUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ExternalProfileUrl))
                return "";
            if (ExternalProfileSystem == Goodreads)
                return Regex.Match(ExternalProfileUrl, @"show/(\d+)").Groups[1].Value;
            if (ExternalProfileSystem == Storygraph)
                return Regex.Match(ExternalProfileUrl, @"profile/(.+)").Groups[1].Value;
            return "";
        }
    }
    public string? Language { get; set; }
    public string? AvatarUrl { get; set; }
    public string? AboutMe { get; set; }
    public string RenderedAboutMe { get => AboutMe?.RenderMarkdown() ?? ""; }
    public bool LoginEnabled { get; set; }
    public bool IsBot { get; set; }
    public bool IsActiveParticipant => LoginEnabled && !IsBot;
    public ICollection<NewsletterSubscription>? NewsletterSubscriptions { get; set; }
    public ExternalBookSystem? ExternalProfileSystem { get; set; } = Goodreads;

}
