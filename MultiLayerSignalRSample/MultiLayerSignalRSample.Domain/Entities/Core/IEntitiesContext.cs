using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IEntitiesContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : class;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
