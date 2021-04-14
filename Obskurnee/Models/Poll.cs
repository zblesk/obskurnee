
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Obskurnee.Models
{
    [Table("Polls")]
    public class Poll : HeaderData
    {
        public enum LinkKind { Discussion, Book, Poll }
        public record FollowupReference(LinkKind kind, int entityId);

        [Key]
        public int PollId { get; set; }

        public int DiscussionId { get; set; }

        /// <summary>
        /// Only used if IsTiebreaker.
        /// </summary>
        public int PreviousPollId { get; set; }
        
        [NotMapped]
        public FollowupReference FollowupLink
        {
            get => !string.IsNullOrWhiteSpace(FollowupLinkSerialized)
                ? JsonConvert.DeserializeObject<FollowupReference>(FollowupLinkSerialized)
                : null;
            set
            {
                FollowupLinkSerialized = JsonConvert.SerializeObject(value);
            }
        }

        [JsonIgnore]
        public string FollowupLinkSerialized { get; set; }

        public int RoundId { get; set; }
        public Round Round { get; set; }

        public List<Post> Options { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
        public Topic Topic { get; set; }
        public bool IsTiebreaker { get; set; } = false;

        [NotMapped]
        public PollResults Results { get; set; }

        [JsonIgnore]
        public string ResultsSerialized
        {
            set
            {
                Results = !string.IsNullOrWhiteSpace(value)
                           ? JsonConvert.DeserializeObject<PollResults>(value)
                           : null;
            }
            get => JsonConvert.SerializeObject(Results);
        }

        public Poll(string ownerId) : base(ownerId) { }

        public int FindAnyWinningPost() => Results?.Votes.OrderByDescending(vote => vote.Votes).First().PostId ?? 0;

        public IList<int> FindWinningPosts()
        {
            var maxVotes = Results?.Votes.Max(vote => vote.Votes);
            return (from vote in Results?.Votes
                    where vote.Votes == maxVotes
                    select vote.PostId
                    ).ToList();
        }
    }
}
