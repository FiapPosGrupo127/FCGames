using System.Linq.Expressions;
using FCGames.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCGames.Infrastructure.Data.Repositories;

public abstract class BaseExpressionService<T>(ApplicationDBContext context) : object() where T : class, IBaseEntity
{
    protected readonly ApplicationDBContext Context = context;

  protected virtual IQueryable<T> BaseQuery(bool tracking = false)
    {
        var condition = (Expression<Func<T, bool>>)(x => x.Removed == false);
        var query = Context.Set<T>().Where(condition);

        if (tracking)
            query = query.AsTracking();
        else
            query = query.AsNoTracking();
        
        return query.AsQueryable();
    }
}