using FCGames.Domain.Entities;

namespace FCGames.Domain.Interfaces.Security;

public interface ITokenService
{
    string GenerateToken(User user, bool force = false);
}