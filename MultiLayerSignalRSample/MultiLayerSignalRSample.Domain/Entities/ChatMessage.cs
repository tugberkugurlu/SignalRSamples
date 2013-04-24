using System;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class ChatMessage : IEntity<int>
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset ReceivedOn { get; set; }

        public User Sender { get; set; }
    }
}