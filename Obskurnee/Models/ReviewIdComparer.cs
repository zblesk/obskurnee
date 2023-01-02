using System.Diagnostics.CodeAnalysis;

namespace Obskurnee.Models;

public class ReviewIdComparer : IEqualityComparer<ExternalReview>
{
    public bool Equals(ExternalReview x, ExternalReview y)
        => x.ExternalBookId == y.ExternalBookId;

    public int GetHashCode([DisallowNull] ExternalReview review)
        => review.ExternalBookId.GetHashCode();
}
