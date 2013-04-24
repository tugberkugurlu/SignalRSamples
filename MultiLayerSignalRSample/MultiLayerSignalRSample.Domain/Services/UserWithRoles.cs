using System.Collections.Generic;
using MultiLayerSignalRSample.Domain.Entities;

namespace MultiLayerSignalRSample.Domain.Services
{
    public class UserWithRoles
    {
        public User User { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}