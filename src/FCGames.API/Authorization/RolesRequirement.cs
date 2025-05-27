using FCGames.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FCGames.Application.Authorization;

public class RolesRequirement(AccessLevel access) : IAuthorizationRequirement
{
    public AccessLevel AccessLevel { get; } = access;
}