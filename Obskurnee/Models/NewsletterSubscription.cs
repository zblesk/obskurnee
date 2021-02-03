using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obskurnee.Models
{
    public class NewsletterSubscription
    {
        [BsonId]
        public string NewsletterSubscriptionId { get => $"{NewsletterName}-{UserId}"; set { } }
        public string UserId { get; set; }
        public string NewsletterName { get; set; }
    }
}
