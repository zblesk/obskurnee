using Obskurnee.Models;
using System.Security.Claims;

namespace Obskurnee
{
    public static class Helpers
    {
        public static T SetOwner<T>(this T data, ClaimsPrincipal fromUserClaims)  where T : HeaderData
        {
            data.OwnerId = fromUserClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            return data;
        }

        public static string GetUserId(this ClaimsPrincipal userClaims) => userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
