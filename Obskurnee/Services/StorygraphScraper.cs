using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System.IO;
using Flurl.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using static Obskurnee.Models.ExternalReview.ReviewKind;
using static Obskurnee.Models.ExternalBookSystem;

namespace Obskurnee.Services;

public class StorygraphScraper : IExternalBookScraper
{
    private static readonly Random _rand = new();
    private readonly ILogger<GoodreadsScraper> _logger;
    private readonly Config _config;
    private readonly ApplicationDbContext _db;
    public ExternalBookSystem ExternalSystem => Storygraph;

    public StorygraphScraper(
        ILogger<GoodreadsScraper> logger,
        Config config,
        ApplicationDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    //public async Task<GoodreadsBookInfo> ScrapeBookInfo(string storygraphUrl)
    //{
    //    if (string.IsNullOrWhiteSpace(storygraphUrl)
    //        || !storygraphUrl.StartsWith("https://app.thestorygraph.com"))
    //    {
    //        throw new Exception("Invalid URL: " + storygraphUrl);
    //    }
    //    _logger.LogInformation("Scraping {url}", storygraphUrl);
    //    var result = new GoodreadsBookInfo("none") { Url = storygraphUrl };
    //    try
    //    {
    //        var bookPageHtml = await storygraphUrl.GetStringAsync();
    //        var imgUrl = ExtractBookInfo(storygraphUrl, result, bookPageHtml);
    //        await ExtractBookImage(result, imgUrl);
    //        _db.BookInfos.Add(result);
    //        await _db.SaveChangesAsync();
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "GR extraction failed for {url}", storygraphUrl);
    //        return null;
    //    }
    //}

    public IEnumerable<ExternalReview> GetCurrentlyReadingBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.ExternalProfileUserId))
        {
            _logger.LogDebug("Triggered RSS feed fetch for user {userId}, but no External Profile User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<ExternalReview>();
        }

        var profileUrl = $"https://app.thestorygraph.com/currently-reading/{user.ExternalProfileUserId}";
        var promise = GetReviewsFromFeed(user.Id, profileUrl, CurrentlyReading);
        promise.Wait();
        return promise.Result;
    }

    public IEnumerable<ExternalReview> GetReadBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.ExternalProfileUserId))
        {
            _logger.LogInformation("Triggered RSS feed fetch for user {userId}, but no External Profile User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<ExternalReview>();
        }

        var profileUrl = $"https://app.thestorygraph.com/books-read/{user.ExternalProfileUserId}";
        var promise = GetReviewsFromFeed(user.Id, profileUrl, Read);
        promise.Wait();
        return promise.Result;
    }

    private async Task<List<ExternalReview>> GetReviewsFromFeed(
        string userId,
        string rssUrl,
        ExternalReview.ReviewKind kind)
    {
        var response = await rssUrl
            .GetStringAsync();

        var html = new HtmlDocument();
        html.LoadHtml(response);


        var attribs = from book in html.DocumentNode.QuerySelectorAll(@".book-pane-content")
                      let img = book.QuerySelector("img").Attributes["src"].Value
                      let links = book.QuerySelectorAll(".book-title-author-and-series a")
                          .Select(e => (text: e.InnerText, link: e.Attributes["href"].Value))
                      select new { image = img, links = links };


        var reviews = (from book in attribs
                       let title = book.links.First().text
                       let bookId = book.links.First().link.Replace("/books/", "")
                       let authors = string.Join(", ", book.links.Where(a => a.link.StartsWith("/authors")).Select(a => a.text))
                       let series = string.Join(" ", book.links.Where(a => a.link.StartsWith("/series")).Select(a => a.text))
                       select new ExternalReview(userId)
                       {
                           ReviewId = $"{userId}-SG-{bookId}",
                           Title = title,
                           ImageUrl = book.image,
                           Author = authors,
                           //Rating = TryGetUshort(rating),
                           //ReviewText = GetElementExtensionValueByOuterName(item, "user_review"),
                           ExternalBookId = bookId,
                           //ReviewUrl = item.Id,
                           Kind = kind,
                           Series = series,
                           ExternalSystem = Storygraph
                       })
                       .ToList();
        _logger.LogDebug("Loaded {count} items for user {userId} from RSS {rssUrl}",
            reviews.Count,
            userId,
            rssUrl);
        return reviews;
    }
}
