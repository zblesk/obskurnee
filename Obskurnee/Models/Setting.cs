using LiteDB;

namespace Obskurnee.Models
{
    public class Setting
    {
        public class Keys
        {
            public const string MailgunSettings = "mailgun-creds";
            public const string ModNoticeboard = "noticeboard-contents";
        }

        [BsonId]
        public string Key { get; set; }
        public dynamic Value { get; set; }
    }
}
