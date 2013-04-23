using GenericRepository.EntityFramework;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public class AsyncEntitiesContext : EntitiesContext, IAsyncEntitiesContext
    {
    }
}