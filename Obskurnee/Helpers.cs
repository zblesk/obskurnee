using Obskurnee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Obskurnee
{
    public static class Helpers
    {
        public static T SetOwner<T>(this T data, ClaimsPrincipal fromUserClaims)  where T : HeaderData
        {
            data.OwnerId = fromUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            return data;
        }

        public static string GetUserId(this ClaimsPrincipal userClaims) => ((ClaimsIdentity)userClaims.Identity).FindFirst("Id").Value;
    }
}
