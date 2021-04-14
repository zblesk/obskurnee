
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obskurnee.Models
{
    [Table("NewsletterSubscriptions")]
    [Index(nameof(NewsletterName))]
    [Index(nameof(UserId))]
    public class NewsletterSubscription
    {
        [Key] public string NewsletterSubscriptionId { get => $"{NewsletterName}-{UserId}"; set { } }
        public string NewsletterName { get; set; }
        public string UserId { get; set; }
        public Bookworm User { get; set; }
    }
}
