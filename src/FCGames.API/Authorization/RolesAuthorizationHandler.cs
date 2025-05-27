using System.Security.Claims;
using FCGames.Domain.Enums;
using FCGames.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FCGames.Application.Authorization;

public class RolesAuthorizationHandler : AuthorizationHandler<RolesRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
    {
        var roleClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

        if (roleClaim.IsNullOrEmpty() == false && Enum.TryParse<AccessLevel>(roleClaim, out var userRoles))
        {
            if (requirement.AccessLevel.HasAnyFlag(userRoles)) 
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}