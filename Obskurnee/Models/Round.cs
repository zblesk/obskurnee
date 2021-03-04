using LiteDB;

namespace Obskurnee.Models
{
    public class Round : HeaderData
    {

        [BsonId] public int RoundId { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
        public int ThemeDiscussionId { get; set; }
        public int ThemePollId { get; set; }
        public int BookDiscussionId { get; set; }
        public int BookPollId { get; set; }
        public Topic Kind { get; set; }

        ///<remarks>Will not be auto-loaded from DB.</remarks>
        [BsonIgnore] public Book Book { get; set; }

        public Round(string ownerId) : base(ownerId)
        {
        }
    }
}
