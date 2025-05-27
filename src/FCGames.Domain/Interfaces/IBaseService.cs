using FCGames.Domain.Entities.Interfaces;
using FCGames.Domain.Interfaces.Infraestructure;

namespace FCGames.Domain.Interfaces;

public interface IBaseService<T> : IRepository<T> where T : class, IBaseEntity
{
    Task Remove(T entity);
}