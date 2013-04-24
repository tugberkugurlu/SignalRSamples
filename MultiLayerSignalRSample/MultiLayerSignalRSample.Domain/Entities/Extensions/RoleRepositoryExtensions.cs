using System.Linq;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public static class RoleRepositoryExtensions
    {
        public static Role GetSingleByRoleName(
            this IAsyncEntityRepository<Role> roleRepository, string roleName)
        {
            return roleRepository.GetAll().FirstOrDefault(x => x.Name == roleName);
        }
    }
}
