using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLayerSignalRSample.Domain.Entities {

    public class PrivateChatMessage 
    {
        public int Id { get; set; }
        
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }

        public string Content { get; set; }
        public DateTimeOffset ReceivedOn { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}