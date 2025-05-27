using FCGames.Application.Dto;

namespace FCGames.Application.Interfaces;

public interface ITokenApplicationService
{
    Task<string> GetToken(UserLogin userLogin);
    Task<string> GetTokenByAutorization(string email);
}