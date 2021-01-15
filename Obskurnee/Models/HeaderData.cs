using AspNetCore.Identity.LiteDB.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
