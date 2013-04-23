using System;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset ReceivedOn { get; set; }

        public User Sender { get; set; }
    }
}