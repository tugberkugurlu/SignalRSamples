using Microsoft.AspNet.SignalR;

namespace RedisScaleOutSample.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.messageReceived(message);
        }
    }
}