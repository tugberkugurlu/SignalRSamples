using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IAsyncEntityRepository<TEntity>
        : GenericRepository.EntityFramework.IEntityRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        Task<List<TEntity>> LoadAllAsync();
        Task<List<TEntity>> LoadAllAsync(CancellationToken cancellationToken);

        Task<List<TEntity>> LoadAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> LoadAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<TEntity> GetSingleAsync(int id);
        Task<TEntity> GetSingleAsync(int id, CancellationToken cancellationToken);

        Task<int> SaveAsync();
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}