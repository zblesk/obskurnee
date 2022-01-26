using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models;

[Table("Discussions")]
public class Discussion : HeaderData
{
    [Key]
    public int DiscussionId { get; set; }
    public int PollId { get; set; }
    public int RoundId { get; set; }
    public Round Round { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsClosed { get; set; }
    virtual public Topic Topic { get; set; }
    [NotMapped]

    public string RenderedDescription { get => Description.RenderMarkdown(); }
    public List<Post> Posts { get; set; } = new List<Post>();

    public Discussion(string ownerId) : base(ownerId) { }
}
