namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, int>
            where TEntity : class, IEntity<int>
    {
    }
}
