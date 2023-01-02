using Obskurnee.Models;

namespace Obskurnee.Services;

public interface IExternalBookScraper
{
    IEnumerable<ExternalReview> GetCurrentlyReadingBooks(Bookworm user);
    IEnumerable<ExternalReview> GetReadBooks(Bookworm user);

    public ExternalBookSystem ExternalSystem { get; }
}
