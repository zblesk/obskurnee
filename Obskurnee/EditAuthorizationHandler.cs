using Microsoft.AspNetCore.Authorization;
using Obskurnee.Models;

namespace Obskurnee;

public class EditAuthorizationHandler :
    AuthorizationHandler<MatchingOwnerRequirement, HeaderData>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   MatchingOwnerRequirement requirement,
                                                   HeaderData data)
    {
        if (data == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        if (context.User.Identity.Name == data.OwnerId
            || context.User.IsInRole(BookclubRoles.Admin)
            || context.User.IsInRole(BookclubRoles.Moderator))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public class MatchingOwnerRequirement : IAuthorizationRequirement { }
