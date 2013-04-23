using System.Threading;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IAsyncEntitiesContext : IEntitiesContext
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}