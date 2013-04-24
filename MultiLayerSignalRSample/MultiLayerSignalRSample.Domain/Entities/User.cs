using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(320)]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        public bool IsLocked { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<HubConnection> HubConnections { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<PrivateChatMessage> SenderPrivateChatMessages { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<PrivateChatMessage> ReceiverPrivateChatMessages { get; set; }
    }
}