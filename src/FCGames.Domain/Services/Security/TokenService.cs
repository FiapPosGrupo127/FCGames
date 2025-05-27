using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FCGames.Domain.Configuration;
using FCGames.Domain.Entities;
using FCGames.Domain.Interfaces.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FCGames.Domain.Services.Security;

public class TokenService(IOptions<TokenConfiguration> options, IMemoryCache cache) : ITokenService
{
    private readonly TokenConfiguration _configuration = options.Value;
    private readonly IMemoryCache _cache = cache;

    public string GenerateToken(User user, bool force = false)
    {
        if (_cache.TryGetValue(user.Id, out string token) && force == false)
            return token;
        else
            _cache.Remove(user.Id);

        token = CreateToken(user);

        var cacheOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_configuration.ExpirationTimeHour),
            SlidingExpiration = TimeSpan.FromMinutes(_configuration.IncreaseExpirationTimeMinutes)
        };

        _cache.Set(user.Id, token, cacheOptions);

        return token;
    }

    private string CreateToken(User user)
    {
        var jwtKey = _configuration.Key;
        if (string.IsNullOrEmpty(jwtKey))
            throw new Exception("JWT Key is not configured.");

        var key = Convert.FromBase64String(jwtKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((int)user.AccessLevel).ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(_configuration.ExpirationTimeHour),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}