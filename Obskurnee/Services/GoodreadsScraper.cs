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
using static Obskurnee.Models.GoodreadsReview.ReviewKind;

namespace Obskurnee.Services;

public class GoodreadsScraper
{
    private static readonly Random _rand = new();
    private readonly ILogger<GoodreadsScraper> _logger;
    private readonly Config _config;
    private readonly ApplicationDbContext _db;

    public GoodreadsScraper(
        ILogger<GoodreadsScraper> logger,
        Config config,
        ApplicationDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<GoodreadsBookInfo> ScrapeBookInfo(string goodreadsUrl)
    {
        if (string.IsNullOrWhiteSpace(goodreadsUrl)
            || !goodreadsUrl.StartsWith("https://www.goodreads.com"))
        {
            throw new Exception("Invalid URL: " + goodreadsUrl);
        }
        _logger.LogInformation("Scraping {url}", goodreadsUrl);
        var result = new GoodreadsBookInfo("none") { Url = goodreadsUrl };
        try
        {
            var bookPageHtml = await goodreadsUrl.GetStringAsync();
            var imgUrl = ExtractBookInfo(goodreadsUrl, result, bookPageHtml);
            await ExtractBookImage(result, imgUrl);
            _db.BookInfos.Add(result);
            await _db.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GR extraction failed for {url}", goodreadsUrl);
            return null;
        }
    }

    public IEnumerable<GoodreadsReview> GetCurrentlyReadingBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.GoodreadsUserId))
        {
            _logger.LogDebug("Triggered RSS feed fetch for user {userId}, but no Goodreads User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<GoodreadsReview>();
        }

        var rssUrl = $"{_config.GoodreadsRssBaseUrl}{user.GoodreadsUserId}?shelf=currently-reading";
        return GetReviewsFromFeed(user.Id, rssUrl, CurrentlyReading);
    }

    public IEnumerable<GoodreadsReview> GetReadBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.GoodreadsUserId))
        {
            _logger.LogInformation("Triggered RSS feed fetch for user {userId}, but no Goodreads User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<GoodreadsReview>();
        }

        var rssUrl = $"{_config.GoodreadsRssBaseUrl}{user.GoodreadsUserId}?shelf=read";
        return GetReviewsFromFeed(user.Id, rssUrl, Read);
    }

    private List<GoodreadsReview> GetReviewsFromFeed(
        string userId,
        string rssUrl,
        GoodreadsReview.ReviewKind kind)
    {
        var reader = XmlReader.Create(rssUrl);
        var feed = SyndicationFeed.Load(reader);
        var reviews = (from item in feed.Items
                       let rating = GetElementExtensionValueByOuterName(item, "user_rating")
                       let bookId = GetElementExtensionValueByOuterName(item, "book_id")
                       select new GoodreadsReview(userId)
                       {
                           ReviewId = $"{userId}-{bookId}",
                           BookTitle = item.Title.Text.Trim(),
                           ImageUrl = GetElementExtensionValueByOuterName(item, "book_large_image_url"),
                           Author = GetElementExtensionValueByOuterName(item, "author_name"),
                           Rating = TryGetUshort(rating),
                           ReviewText = GetElementExtensionValueByOuterName(item, "user_review"),
                           GoodreadsBookId = bookId,
                           ReviewUrl = item.Id,
                           Kind = kind
                       })
                       .ToList();
        _logger.LogDebug("Loaded {count} items for user {userId} from RSS {rssUrl}",
            reviews.Count,
            userId,
            rssUrl);
        return reviews;
    }

    private static string GetElementExtensionValueByOuterName(SyndicationItem item, string outerName)
    {
        if (item.ElementExtensions.All(x => x.OuterName != outerName)) return null;
        return item.ElementExtensions.Single(x => x.OuterName == outerName).GetObject<XElement>().Value;
    }

    private static ushort TryGetUshort(string number)
        => ushort.TryParse(number, out var result) ? result : default;

    private string ExtractBookInfo(
        string goodreadsUrl,
        GoodreadsBookInfo result,
        string bookPageHtml)
    {
        var converter = new ReverseMarkdown.Converter();
        var html = new HtmlDocument();
        html.LoadHtml(bookPageHtml);

        var document = html.DocumentNode;

        result.Title = document.QuerySelector("#bookTitle")?.InnerText?.Trim();
        result.Author = document.QuerySelector(".authorName__container > a:nth-child(1) > span:nth-child(1)")?.InnerText?.Trim();
        var description = document.QuerySelector("#description.readable.stacked span:nth-child(2)")?.InnerHtml;
        if (description != null)
        {
            result.Description = converter.Convert(description);
        }
        else
        {
            _logger.LogWarning("Failed extracting Description from {url}", goodreadsUrl);
        }
        var pages = document.QuerySelectorAll("#details > div:nth-child(1) > span").FirstOrDefault(node => node.InnerText.Contains("pages"))?.InnerText;
        var imgUrl = document.QuerySelector(".editionCover > img")?.Attributes["src"]?.Value;

        if (!string.IsNullOrWhiteSpace(pages)
            && pages.IndexOf(' ') > -1)
        {
            if (int.TryParse(pages[..pages.IndexOf(' ')], out var pageNum))
            {
                result.PageCount = pageNum;
            }
        }

        return imgUrl;
    }

    private async Task ExtractBookImage(
        GoodreadsBookInfo result,
        string imgUrl)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                _logger.LogInformation("Downloading image for {bookname} at {url}", result.Title, imgUrl);
                var pic = await imgUrl.GetBytesAsync();
                var sanitizedName = Regex.Replace(
                        result.Title.Replace(' ', '-'),
                        "[^a-zA-Z0-9-]", "");
                var relativeFilename = sanitizedName[..Math.Min(90, sanitizedName.Length)]
                    + _rand.Next(10_000, 100_000).ToString()
                    + Path.GetExtension(imgUrl) ?? ".jpg";
                var relativeUrl = Helpers.MakeImageRelativePath(relativeFilename);
                _logger.LogInformation("File downloaded, saving it as {filename}", relativeFilename);
                await _db.Images.AddAsync(new StoredImage()
                {
                    FileName = relativeFilename,
                    Extension = Path.GetExtension(relativeFilename),
                    FileContents = pic,
                    Kind = StoredImage.ImageKind.BookCover,
                });
                await _db.SaveChangesAsync();
                result.ImageUrl = relativeUrl;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Image extraction failed for {url}", imgUrl);
        }
    }
}
