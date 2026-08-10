using System.Collections.Generic;

namespace Obskurnee.ViewModels;

public class StatsOverview
{
    public List<UserStats> UserStats { get; set; } = new();
    public List<BookRatingStats> BookStats { get; set; } = new();
}
