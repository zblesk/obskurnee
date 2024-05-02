using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models;

[Table("GoodreadsBookInfos")]
public class GoodreadsBookInfo(string ownerId) : HeaderData(ownerId)
{
    [Key]
    public int GoodreadsBookInfoId { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? PageCount { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
