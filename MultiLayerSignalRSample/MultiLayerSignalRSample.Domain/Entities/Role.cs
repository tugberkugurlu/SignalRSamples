using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GenericRepository;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class Role : IEntity<int>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }

        public Role()
        {
            UserInRoles = new HashSet<UserInRole>();
        }
    }
}
