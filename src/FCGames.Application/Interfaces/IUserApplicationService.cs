using FCGames.Application.Dto;

namespace FCGames.Application.Interfaces;

public interface IUserApplicationService
{
    Task<User> Add(GuestUser model);
}