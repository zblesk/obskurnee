using Obskurnee.Models;
using Obskurnee.Services;
using System.Security.Claims;

namespace Obskurnee.ViewModels
{
    public record UserInfo(
        string Id, 
        string Name, 
        string Email, 
        string IsModerator = "false", 
        string IsAdmin = "false", 
        string Token = null)
    {
        public static UserInfo FromPrincipal(ClaimsPrincipal principal, string token = null)
            => new(
                principal.GetUserId(),
                principal.Identity.Name,
                principal.FindFirstValue(ClaimTypes.Email),
                principal.FindFirstValue(BookclubClaims.Moderator) ?? "false",
                principal.FindFirstValue(BookclubClaims.Admin) ?? "false",
                token);

        public static UserInfo FromBookworm(Bookworm user)
            => new(
                user.Id,
                user.UserName,
                user.Email.Address);
    }
}
