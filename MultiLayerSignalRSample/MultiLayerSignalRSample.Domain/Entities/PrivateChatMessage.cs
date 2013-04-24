using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [InverseProperty("SenderPrivateChatMessages")]
        public User Sender { get; set; }

        [InverseProperty("ReceiverPrivateChatMessages")]
        public User Receiver { get; set; }
    }
}