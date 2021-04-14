
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Obskurnee.Models
{
    [Table("Rounds")]
    public class Round : HeaderData
    {
        [Key] 
        public int RoundId { get; set; }
        public Topic Kind { get; set; }
        public string Title { get; set; }

        [ForeignKey(nameof(ThemeDiscussion))]
        public int? ThemeDiscussionId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ThemeDiscussionId))]
        public Discussion ThemeDiscussion { get; set; }

        [ForeignKey(nameof(ThemePoll))]
        public int? ThemePollId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ThemePollId))]
        public Poll ThemePoll { get; set; }

        [ForeignKey(nameof(ThemeTiebreakerPoll))]
        public int? ThemeTiebreakerPollId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ThemeTiebreakerPollId))]
        public Poll ThemeTiebreakerPoll { get; set; }

        [ForeignKey(nameof(BookDiscussion))]
        public int? BookDiscussionId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BookDiscussionId))]
        public Discussion BookDiscussion { get; set; }

        [ForeignKey(nameof(BookPoll))]
        public int? BookPollId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BookPollId))]
        public Poll BookPoll { get; set; }

        [ForeignKey(nameof(BookTiebreakerPoll))]
        public int? BookTiebreakerPollId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(BookTiebreakerPollId))]
        public Poll BookTiebreakerPoll { get; set; }

        public int? BookId { get; set; }

        public Book Book { get; set; }

        [InverseProperty("Round")]
        [JsonIgnore]
        public List<Discussion> AllRelatedDiscussions { get; set; }

        [InverseProperty("Round")]
        [JsonIgnore]
        public List<Poll> AllRelatedPolls { get; set; }

        public Round(string ownerId) : base(ownerId)
        {
        }
    }
}
