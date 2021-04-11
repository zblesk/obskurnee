using LiteDB;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("Settings")]
    public class Setting
    {
        public static class Keys
        {
            public const string MailgunSettings = "mailgun-creds";
            public const string ModNoticeboard = "noticeboard-contents";
        }

        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime LastChange { get; set; } = DateTime.UtcNow;
    }
}
