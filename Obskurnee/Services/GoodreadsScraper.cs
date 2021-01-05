using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class GoodreadsScraper
    {
        private static Random _rand = new Random();
        private readonly ILogger<GoodreadsScraper> _logger;
        private readonly IWebHostEnvironment _hostEnv;

        public GoodreadsScraper(ILogger<GoodreadsScraper> logger, IWebHostEnvironment hostEnv)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hostEnv = hostEnv ?? throw new ArgumentNullException(nameof(hostEnv));
        }

        public async Task<GoodreadsBookInfo> Scrape(string goodreadsUrl)
        {
            if (string.IsNullOrWhiteSpace(goodreadsUrl)
                || !goodreadsUrl.StartsWith("https://www.goodreads.com"))
            {
                throw new Exception("Invalid URL: " + goodreadsUrl);
            }
            _logger.LogInformation("Scraping {url}", goodreadsUrl);
            var result = new GoodreadsBookInfo { Url = goodreadsUrl };
            try
            {
                var converter = new ReverseMarkdown.Converter();
                using (WebClient client = new WebClient())
                {
                    string downloadString = await client.DownloadStringTaskAsync(goodreadsUrl);
                    var html = new HtmlDocument();
                    html.LoadHtml(downloadString);

                    var document = html.DocumentNode;

                    result.Name = document.QuerySelector("#bookTitle")?.InnerText?.Trim();
                    result.Author = document.QuerySelector(".authorName__container > a:nth-child(1) > span:nth-child(1)")?.InnerText?.Trim();
                    result.Description = converter.Convert(document.QuerySelector("#description.readable.stacked span:nth-child(2)").InnerHtml);
                    var pages = document.QuerySelector("#details > div:nth-child(1) > span:nth-child(2)")?.InnerText;
                    var imgUrl = document.QuerySelector(".editionCover > img")?.Attributes["src"]?.Value;

                    if (!string.IsNullOrWhiteSpace(pages)
                        && pages.IndexOf(' ') > -1)
                    {
                        if (int.TryParse(pages.Substring(0, pages.IndexOf(' ')), out int pageNum))
                        {
                            result.Pages = pageNum;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(imgUrl))
                        {
                            _logger.LogInformation("Downloading image for {bookname} at {url}", result.Name, imgUrl);
                            var pic = await client.DownloadDataTaskAsync(imgUrl);
                            var sanitizedName = Regex.Replace(
                                    result.Name.Replace(' ', '-'),
                                    "[^a-zA-Z0-9-]", "");
                            var relativePath = "/images/" 
                                + sanitizedName.Substring(0, Math.Min(90, sanitizedName.Length)) 
                                + _rand.Next(10_000, 100_000).ToString()
                                + Path.GetExtension(imgUrl) ?? ".jpg";
                            var fName = Path.Join(_hostEnv.ContentRootPath, 
                                relativePath);
                            _logger.LogInformation("File downloaded, saving it at {filename}", fName);
                            await File.WriteAllBytesAsync(fName, pic);
                            result.ImagePath = relativePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Image extraction failed for {url}", imgUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GR extraction failed for {url}", goodreadsUrl);
            }
            return result;
        }
    }
}
