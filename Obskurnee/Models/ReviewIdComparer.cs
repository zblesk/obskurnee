using System.Diagnostics.CodeAnalysis;

namespace Obskurnee.Models;

public class ReviewIdComparer : IEqualityComparer<GoodreadsReview>
{
    public bool Equals(GoodreadsReview x, GoodreadsReview y)
        => x.GoodreadsBookId == y.GoodreadsBookId;

    public int GetHashCode([DisallowNull] GoodreadsReview review)
        => review.GoodreadsBookId.GetHashCode();
}
