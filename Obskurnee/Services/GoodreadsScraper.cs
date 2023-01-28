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

public class GoodreadsScraper : IExternalBookScraper
{
    private static readonly Random _rand = new();
    private readonly ILogger<GoodreadsScraper> _logger;
    private readonly Config _config;
    private readonly ApplicationDbContext _db;

    public ExternalBookSystem ExternalSystem => Goodreads;

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
            var bookPageHtml = await goodreadsUrl
                .WithHeader("Cache-Control", "max-age=0")
                .WithHeader("sec-ch-ua", "\"Microsoft Edge\";v=\"107\", \"Chromium\";v=\"107\", \"Not = A ? Brand\";v=\"24\"")
                .WithHeader("sec-ch-ua-mobile", "?0")
                .WithHeader("sec-ch-ua-platform", "\"Windows\"")
                .WithHeader("Upgrade-Insecure-Requests", "1")
                .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Edg/107.0.1418.56")
                .WithHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3; q = 0.9")
                .WithHeader("Sec-Fetch-Site", "none")
                .WithHeader("Sec-Fetch-Mode", "navigate")
                .WithHeader("Sec-Fetch-User", "?1")
                .WithHeader("Sec-Fetch-Dest", "document")
                .WithHeader("Accept-Encoding", "gzip, deflate, br")
                .WithHeader("Accept-Language", "en-US,en;q=0.9")
                .GetStringAsync();
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

    public IEnumerable<ExternalReview> GetCurrentlyReadingBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.ExternalProfileUserId))
        {
            _logger.LogDebug("Triggered RSS feed fetch for user {userId}, but no External Profile User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<ExternalReview>();
        }

        var rssUrl = $"{_config.GoodreadsRssBaseUrl}{user.ExternalProfileUserId}?shelf=currently-reading";
        return GetReviewsFromFeed(user.Id, rssUrl, CurrentlyReading);
    }

    public IEnumerable<ExternalReview> GetReadBooks(Bookworm user)
    {
        if (string.IsNullOrWhiteSpace(user.ExternalProfileUserId))
        {
            _logger.LogInformation("Triggered RSS feed fetch for user {userId}, but no External Profile User ID is available. Exitting.", user.Id);
            return Enumerable.Empty<ExternalReview>();
        }

        var rssUrl = $"{_config.GoodreadsRssBaseUrl}{user.ExternalProfileUserId}?shelf=read";
        return GetReviewsFromFeed(user.Id, rssUrl, Read);
    }

    private List<ExternalReview> GetReviewsFromFeed(
        string userId,
        string rssUrl,
        ExternalReview.ReviewKind kind)
    {
        var reader = XmlReader.Create(rssUrl);
        var feed = SyndicationFeed.Load(reader);
        var reviews = (from item in feed.Items
                       let rating = GetElementExtensionValueByOuterName(item, "user_rating")
                       let bookId = GetElementExtensionValueByOuterName(item, "book_id")
                       select new ExternalReview(userId)
                       {
                           ReviewId = $"{userId}-{bookId}",
                           Title = item.Title.Text.Trim(),
                           ImageUrl = GetElementExtensionValueByOuterName(item, "book_large_image_url"),
                           Author = GetElementExtensionValueByOuterName(item, "author_name"),
                           Rating = TryGetUshort(rating),
                           ReviewText = GetElementExtensionValueByOuterName(item, "user_review"),
                           ExternalBookId = bookId,
                           ReviewUrl = item.Id,
                           Kind = kind,
                           ExternalSystem = Goodreads,
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
        if (result.Title == null)
        {
            //second attempt, different layout
            result.Title = document.QuerySelector(".Text__title1")?.InnerText.Trim();
        }

        result.Author = document.QuerySelector(".authorName__container > a:nth-child(1) > span:nth-child(1)")?.InnerText?.Trim();
        if (result.Author == null)
        {
            //second attempt, different layout
            result.Author = document.QuerySelector(".ContributorLinksList > span:nth-child(1) > a:nth-child(1) > span:nth-child(1)")?.InnerText?.Trim();
        }

        var description = document.QuerySelector("#description.readable.stacked span:nth-child(2)")?.InnerHtml;
        if (description == null)
        {
            //second attempt, different layout
            description = document.QuerySelector(".TruncatedContent__text--large > div:nth-child(1) > div:nth-child(1) > span:nth-child(1)")?.InnerHtml;
        }
        if (description != null)
        {
            result.Description = converter.Convert(description);
        }
        else
        {
            _logger.LogWarning("Failed extracting Description from {url}", goodreadsUrl);
        }

        var pages = document.QuerySelectorAll("#details > div:nth-child(1) > span").FirstOrDefault(node => node.InnerText.Contains("pages"))?.InnerText;
         if (pages == null)
        {
            //second attempt, different layout
            pages = document.QuerySelectorAll(".FeaturedDetails > p:nth-child(1)").FirstOrDefault(node => node.InnerText.Contains("pages"))?.InnerText;
        }
        if (!string.IsNullOrWhiteSpace(pages)
            && pages.IndexOf(' ') > -1)
        {
            if (int.TryParse(pages[..pages.IndexOf(' ')], out var pageNum))
            {
                result.PageCount = pageNum;
            }
        }

        var imgUrl = document.QuerySelector(".editionCover > img")?.Attributes["src"]?.Value;
        if (imgUrl == null)
        {
            imgUrl = document.QuerySelector(".ResponsiveImage")?.Attributes["src"]?.Value;
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
