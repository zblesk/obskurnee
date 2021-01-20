using Obskurnee.Models;
using Obskurnee.Services;
using System.Security.Claims;

namespace Obskurnee.ViewModels
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string IsModerator { get; set; } = "false";
        public string IsAdmin { get; set; } = "false";
        public string Token { get; set; } = null;

        public UserInfo() { }

        public UserInfo(string id, string name, string email) 
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public static UserInfo FromPrincipal(ClaimsPrincipal principal, string token = null)
                => (principal?.Identity == null || !principal.Identity.IsAuthenticated)
                    ? new UserInfo()
                    : new UserInfo()
                    {
                        Id = principal.GetUserId(),
                        Name = principal.Identity.Name,
                        Email = principal.FindFirstValue(ClaimTypes.Email),
                        IsModerator = principal.FindFirstValue(BookclubClaims.Moderator)?.ToString() ?? "false",
                        IsAdmin = principal.FindFirstValue(BookclubClaims.Admin)?.ToString() ?? "false",
                        Token = token,
                    };

        public static UserInfo FromBookworm(Bookworm user)
            => new(
                user.Id,
                user.UserName,
                user.Email.Address);
    }
}
