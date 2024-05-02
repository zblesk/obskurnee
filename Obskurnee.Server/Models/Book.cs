using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models;

[Table("Books")]
public class Book(string ownerId) : HeaderData(ownerId)
{
    [Key]
    public int BookId { get; set; }
    public int? PostId { get; set; }
    public Post? Post { get; set; }

    [ForeignKey("Round")]
    public int? RoundId { get; set; }
    public Round? Round { get; set; }
    public int Order { get; set; }
    public int? BookDiscussionId { get; set; }
    public int? BookPollId { get; set; }
}
