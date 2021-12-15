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
        public string AboutMeHtml { get; set; }
        public bool IsModerator { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public string Token { get; set; } = null;
        public string GoodreadsUrl { get; set; }
        public string Language { get; set; }
        public string AvatarUrl { get; set; }
        public bool LoginEnabled { get; set; }
        public bool IsBot { get; set; }
        public bool IsActiveParticipant { get; set; }

        public static UserInfo From(ClaimsPrincipal principal, string includeToken = null)
                => (principal?.Identity == null || !principal.Identity.IsAuthenticated)
                    ? new UserInfo()
                    : new UserInfo()
                    {
                        UserId = principal.GetUserId(),
                        Name = principal.Identity.Name,
                        Email = principal.FindFirstValue(ClaimTypes.Email),
                        IsModerator = principal.FindFirstValue(BookclubClaims.Moderator) != null,
                        IsAdmin = principal.FindFirstValue(BookclubClaims.Admin) != null,
                        LoginEnabled = true,
                        IsBot = principal.FindFirstValue(BookclubClaims.Bot) != null,
                        IsActiveParticipant = principal.FindFirstValue(BookclubClaims.Bot) == null,
                        Token = includeToken,
                    };

        public static UserInfo From(Bookworm user, ClaimsPrincipal principal = null)
            => new (){
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                AboutMe = user.AboutMe,
                AboutMeHtml = user.RenderedAboutMe,
                GoodreadsUrl = user.GoodreadsProfileUrl,
                Language = user.Language,
                AvatarUrl = user.AvatarUrl,
                IsBot = user.IsBot,
                LoginEnabled = user.LoginEnabled,
                IsActiveParticipant = user.IsActiveParticipant,
                IsModerator = principal?.FindFirstValue(BookclubClaims.Moderator) != null,
                IsAdmin = principal?.FindFirstValue(BookclubClaims.Admin) != null,
            };


        public static UserInfo From(Bookworm user, IList<Claim> claims)
            => new()
            {
                UserId = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                AboutMe = user.AboutMe,
                AboutMeHtml = user.RenderedAboutMe,
                GoodreadsUrl = user.GoodreadsProfileUrl,
                Language = user.Language,
                AvatarUrl = user.AvatarUrl,
                IsBot = user.IsBot,
                LoginEnabled = user.LoginEnabled,
                IsActiveParticipant = user.IsActiveParticipant,
                IsModerator = claims.Any(claim => claim.Type == BookclubClaims.Moderator),
                IsAdmin = claims.Any(claim => claim.Type == BookclubClaims.Admin),
            };
    }
}
