using LiteDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("Rounds")]
    public class Round : HeaderData
    {

        [BsonId] [Key] public int RoundId { get; set; }
        public Topic Kind { get; set; }
        public string Title { get; set; }
        public int ThemeDiscussionId { get; set; }
        public Discussion ThemeDiscussion { get; set; }
        public int ThemePollId { get; set; }
        public Poll ThemePoll { get; set; }
        public int ThemeTiebreakerPollId { get; set; }
        public Poll ThemeTiebreakerPoll { get; set; }
        public int BookDiscussionId { get; set; }
        public Discussion BookDiscussion { get; set; }
        public int BookPollId { get; set; }
        public Poll BookPoll { get; set; }
        public int BookTiebreakerPollId { get; set; }
        public Poll BookTiebreakerPoll { get; set; }

        public int BookId { get; set; }
        [BsonIgnore] public Book Book { get; set; }

        public Round(string ownerId) : base(ownerId)
        {
        }
    }
}
