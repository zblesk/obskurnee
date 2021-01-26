using LiteDB;
using Obskurnee.Services;
using System;

namespace Obskurnee.Models
{
    public class HeaderData
    {
        public string OwnerId { get; set; } 
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
        [BsonIgnore] public string OwnerName { get => UserService.GetUserName(OwnerId); }

        public HeaderData(string ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
