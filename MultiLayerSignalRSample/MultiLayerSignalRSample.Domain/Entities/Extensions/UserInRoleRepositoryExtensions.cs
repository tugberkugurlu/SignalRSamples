using System.Linq;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public static class UserInRoleRepositoryExtensions
    {
        public static bool IsUserInRole(
            this IEntityRepository<UserInRole> userInRoleRepository,
            int userId,
            int roleId)
        {
            return userInRoleRepository.GetAll()
                .Any(x => x.UserId == userId && x.RoleId == roleId);
        }
    }
}
