namespace Obskurnee.ViewModels;

public class BookPostStats
{
    public int BookId { get; set; }
    public int OriginatingDiscussionId { get; set; }
    public int OriginatingPostId { get; set; }
    public int WinningVotes { get; set; }
    public int TotalVotes { get; set; }
    public int ReviewCount { get; set; }
    public float AverageRating { get; set; }

}
