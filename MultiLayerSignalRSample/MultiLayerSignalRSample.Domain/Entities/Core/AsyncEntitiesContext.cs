using GenericRepository.EntityFramework;
using System.Data.Entity;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public class AsyncEntitiesContext : EntitiesContext, IAsyncEntitiesContext
    {
        public IDbSet<User> Users { get; set; }
        public IDbSet<ChatMessage> ChatMessages { get; set; }
        public IDbSet<PrivateChatMessage> PrivateChatMessages { get; set; }
        public IDbSet<HubConnection> HubConnections { get; set; }
    }
}