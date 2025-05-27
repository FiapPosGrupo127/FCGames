using Microsoft.AspNetCore.Mvc.Filters;

namespace FCGames.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipUserFilterAttribute : Attribute, IFilterMetadata { }