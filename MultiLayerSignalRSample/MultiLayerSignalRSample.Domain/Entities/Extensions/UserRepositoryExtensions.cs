using System.Linq;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public static class UserRepositoryExtensions
    {
        public static User GetSingleByUsername(
            this IAsyncEntityRepository<User> userRepository, string username)
        {
            return userRepository.GetAll().FirstOrDefault(x => x.Name == username);
        }
    }
}