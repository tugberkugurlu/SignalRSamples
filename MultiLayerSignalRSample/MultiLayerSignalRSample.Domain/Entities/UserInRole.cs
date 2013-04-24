using GenericRepository;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class UserInRole : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
