using System.Data.Entity;

namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public class ChatEntitiesContext : EntitiesContextBase
    {
        public IDbSet<ChatMessage> ChatMessages { get; set; }
        public IDbSet<PrivateChatMessage> PrivateChatMessages { get; set; }
        public IDbSet<HubConnection> HubConnections { get; set; }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserInRole> UserInRoles { get; set; }
    }
}