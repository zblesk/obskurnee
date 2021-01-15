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
        [BsonRef("user")]
        public ApplicationUser OwnerId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        //public HeaderData(ApplicationUser owner)
        //{
        //    OwnerId = owner;
        //}
    }
}
