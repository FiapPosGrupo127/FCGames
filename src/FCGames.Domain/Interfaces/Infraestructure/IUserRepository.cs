using FCGames.Domain.Entities;

namespace FCGames.Domain.Interfaces.Infraestructure;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetById(Guid id, bool include = false, bool tracking = false);
    Task<User> GetByEmail(string? email, bool include = false, bool tracking = false);
}