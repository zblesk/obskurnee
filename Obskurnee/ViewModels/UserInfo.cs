using Obskurnee.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Obskurnee.ViewModels
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AboutMe { get; set; }
        public string IsModerator { get; set; } = "false";
        public string IsAdmin { get; set; } = "false";
        public string? Token { get; set; } = null;
        public string GoodreadsUrl { get; set; }

        public static UserInfo From(ClaimsPrincipal principal, string? includeToken = null)
                => (principal?.Identity == null || !principal.Identity.IsAuthenticated)
                    ? new UserInfo()
                    : new UserInfo()
                    {
                        UserId = principal.GetUserId(),
                        Name = principal.Identity.Name,
                        Email = principal.FindFirstValue(ClaimTypes.Email),
                        IsModerator = principal.FindFirstValue(BookclubClaims.Moderator)?.ToString() ?? "false",
                        IsAdmin = principal.FindFirstValue(BookclubClaims.Admin)?.ToString() ?? "false",
                        Token = includeToken,
                    };

        public static UserInfo From(Bookworm user, ClaimsPrincipal principal = null)
            => new (){
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email.Address,
                Phone = user.PhoneNumber,
                AboutMe = user.RenderedAboutMe,
                GoodreadsUrl = user.GoodreadsProfileUrl,
                IsModerator = principal?.FindFirstValue(BookclubClaims.Moderator)?.ToString() ?? "false",
                IsAdmin = principal?.FindFirstValue(BookclubClaims.Admin)?.ToString() ?? "false",
            };


        public static UserInfo From(Bookworm user, IList<Claim> claims)
            => new()
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email.Address,
                Phone = user.PhoneNumber,
                AboutMe = user.RenderedAboutMe,
                GoodreadsUrl = user.GoodreadsProfileUrl,
                IsModerator = claims.Any(claim => claim.Type == BookclubClaims.Moderator).ToString(),
                IsAdmin = claims.Any(claim => claim.Type == BookclubClaims.Admin).ToString(),
            };
    }
}
