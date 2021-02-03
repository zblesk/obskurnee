using Microsoft.AspNetCore.Authorization;
using Obskurnee.Models;
using System.Threading.Tasks;

namespace Obskurnee
{
    public class EditAuthorizationHandler :
        AuthorizationHandler<MatchingOwnerRequirement, HeaderData>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MatchingOwnerRequirement requirement,
                                                       HeaderData data)
        {
            if (context.User.Identity.Name == data.OwnerId
                || context.User.HasClaim(c => c.Type == BookclubClaims.Moderator 
                                                || c.Type == BookclubClaims.Admin))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class MatchingOwnerRequirement : IAuthorizationRequirement { }
}
