using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        private readonly IEntitiesContext _dbContext;

        public EntityRepository(IEntitiesContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public Task<List<TEntity>> LoadAllAsync()
        {
            return LoadAllAsync(CancellationToken.None);
        }

        public Task<List<TEntity>> LoadAllAsync(CancellationToken cancellationToken)
        {
            Task<List<TEntity>> queryTask = GetAll().ToListAsync(cancellationToken);
            return queryTask;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include<TEntity, object>(includeProperty));
        }

        public Task<List<TEntity>> LoadAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return LoadAllIncludingAsync(CancellationToken.None, includeProperties);
        }

        public Task<List<TEntity>> LoadAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            Task<List<TEntity>> queryTask = GetAllIncluding(includeProperties).ToListAsync();
            return queryTask;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> queryable = GetAll().Where<TEntity>(predicate);
            return queryable;
        }

        public Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return LoadByAsync(predicate, CancellationToken.None);
        }

        public Task<List<TEntity>> LoadByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            Task<List<TEntity>> queryTask = FindBy(predicate).ToListAsync(cancellationToken);
            return queryTask;
        }

        public PaginatedList<TEntity> Paginate(int pageIndex, int pageSize)
        {
            PaginatedList<TEntity> paginatedList = Paginate<int>(pageIndex, pageSize, x => x.Id);
            return paginatedList;
        }

        public PaginatedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector)
        {
            return Paginate<TKey>(pageIndex, pageSize, keySelector, null);
        }

        public PaginatedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            PaginatedList<TEntity> paginatedList = Paginate<TKey>(
                pageIndex, pageSize, keySelector, predicate, OrderByType.Ascending, includeProperties);

            return paginatedList;
        }

        public PaginatedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector)
        {
            return PaginateDescending<TKey>(pageIndex, pageSize, keySelector, null);
        }

        public PaginatedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            PaginatedList<TEntity> paginatedList = Paginate<TKey>(
                pageIndex, pageSize, keySelector, predicate, OrderByType.Descending, includeProperties);

            return paginatedList;
        }

        public TEntity GetSingle(int id)
        {
            TEntity entity = GetAll().FirstOrDefault(x => x.Id.Equals(id));
            return entity;
        }

        public Task<TEntity> GetSingleAsync(int id)
        {
            return GetSingleAsync(id, CancellationToken.None);
        }

        public Task<TEntity> GetSingleAsync(int id, CancellationToken cancellationToken)
        {
            Task<TEntity> entityTask = GetAll().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            return entityTask;
        }

        public void Add(TEntity entity)
        {
            _dbContext.SetAsAdded(entity);
        }

        public void AddGraph(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Edit(TEntity entity)
        {
            _dbContext.SetAsModified(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.SetAsDeleted(entity);
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return SaveAsync(CancellationToken.None);
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        // Privates

        private PaginatedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderByType orderByType, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable =
                (orderByType == OrderByType.Ascending)
                    ? GetAllIncluding(includeProperties).OrderBy(keySelector)
                    : GetAllIncluding(includeProperties).OrderByDescending(keySelector);

            queryable = (predicate != null) ? queryable.Where(predicate) : queryable;
            PaginatedList<TEntity> paginatedList = queryable.ToPaginatedList(pageIndex, pageSize);

            return paginatedList;
        }

        private enum OrderByType
        {
            Ascending,
            Descending
        }
    }
}
