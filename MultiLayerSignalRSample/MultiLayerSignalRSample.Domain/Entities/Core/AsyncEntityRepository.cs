using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public class AsyncEntityRepository<TEntity> : 
        EntityRepository<TEntity>, IAsyncEntityRepository<TEntity>
        where TEntity : class, IEntity<int> {

        private readonly IAsyncEntitiesContext _dbContext;

        public AsyncEntityRepository(IAsyncEntitiesContext dbContext)
            : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task<List<TEntity>> LoadAllAsync()
        {
            return GetAll().ToListAsync(CancellationToken.None);
        }

        public Task<List<TEntity>> LoadAllAsync(CancellationToken cancellationToken)
        {
            return GetAll().ToListAsync(cancellationToken);
        }

        public Task<List<TEntity>> LoadAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return LoadAllIncludingAsync(CancellationToken.None, includeProperties);
        }

        public Task<List<TEntity>> LoadAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).ToListAsync(cancellationToken);
        }

        public Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return LoadByAsync(predicate, CancellationToken.None);
        }

        public Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return FindBy(predicate).ToListAsync(cancellationToken);
        }

        public Task<TEntity> GetSingleAsync(int id)
        {
            return GetSingleAsync(id, CancellationToken.None);
        }

        public Task<TEntity> GetSingleAsync(int id, CancellationToken cancellationToken)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public Task<int> SaveAsync()
        {
            return SaveAsync(CancellationToken.None);
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}