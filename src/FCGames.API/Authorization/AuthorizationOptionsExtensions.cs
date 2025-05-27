using FCGames.Application.Authorization;
using FCGames.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FCGames.Infrastructure.Security;

public static class AuthorizationOptionsExtensions
{
    public static AuthorizationOptions AddPolicyWithPermission(this AuthorizationOptions options, string policyName, AccessLevel accessLevel)
    {
        options.AddPolicy(policyName, policy => policy.Requirements.Add(new RolesRequirement(accessLevel)));
        return options;
    }
}