using GenericRepository;

namespace MultiLayerSignalRSample.Domain.Entities
{
    public class HubConnection : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }

        public User User { get; set; }
    }
}