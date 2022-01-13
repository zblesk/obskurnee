using Microsoft.AspNetCore.Authorization;
using Obskurnee.Models;
using System.Security.Claims;
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
                || context.User.HasClaim(c => 
                                        c.Type == ClaimTypes.Role
                                            && (c.Value == BookclubRoles.Moderator 
                                                || c.Value == BookclubRoles.Admin)))
            {
                context.Succeed(requirement);
            }
             
            return Task.CompletedTask;
        }
    }

    public class MatchingOwnerRequirement : IAuthorizationRequirement { }
}
