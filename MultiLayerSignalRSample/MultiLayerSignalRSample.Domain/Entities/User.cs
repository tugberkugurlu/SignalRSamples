using GenericRepository;
using System.Collections.Generic;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }
        public ICollection<PrivateChatMessage> SenderPrivateChatMessages { get; set; }
        public ICollection<PrivateChatMessage> ReceiverPrivateChatMessages { get; set; }
        public ICollection<HubConnection> HubConnections { get; set; }
    }
}