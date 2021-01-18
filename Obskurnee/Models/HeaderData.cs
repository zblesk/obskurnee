using System;

namespace Obskurnee.Models
{
    public class HeaderData
    {
        public string OwnerId { get; set; } 
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

        public HeaderData(string ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
