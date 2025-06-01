using FCGames.Domain.Entities;

namespace FCGames.Domain.Interfaces;

public interface IUserService : IBaseService<User>
{
    Task<User> GetById(Guid id);
    Task<User> GetByEmail(string? email);
}