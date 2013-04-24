using System;
using System.Linq;
using System.Linq.Expressions;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IRepository<TEntity, TId>
            where TEntity : class, IEntity<TId>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        PaginatedList<TEntity> Paginate(int pageIndex, int pageSize);

        TEntity GetSingle(TId id);
    }
}
