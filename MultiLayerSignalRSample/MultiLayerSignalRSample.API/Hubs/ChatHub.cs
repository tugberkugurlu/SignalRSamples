using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using MultiLayerSignalRSample.Domain.Entities.Core;
using MultiLayerSignalRSample.Domain.Entities;
using MultiLayerSignalRSample.Domain.Services;

namespace MultiLayerSignalRSample.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMembershipService _membershipService;
        private readonly IEntityRepository<HubConnection> _hubConnectionRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<ChatMessage> _chatMessageRepository;
        private readonly IEntityRepository<PrivateChatMessage> _privateChatMessageRepository;

        public ChatHub(
            IMembershipService membershipService,
            IEntityRepository<HubConnection> hubConnectionRepository,
            IEntityRepository<User> userRepository,
            IEntityRepository<ChatMessage> chatMessageRepository,
            IEntityRepository<PrivateChatMessage> privateChatMessageRepository)
        {
            _membershipService = membershipService;
            _hubConnectionRepository = hubConnectionRepository;
            _userRepository = userRepository;
            _chatMessageRepository = chatMessageRepository;
            _privateChatMessageRepository = privateChatMessageRepository;
        }

        public void Send(string message)
        {
            string senderName = Context.User.Identity.Name;
            User sender = _userRepository.GetSingleByUsername(senderName);

            ChatMessage chatMessage = new ChatMessage
            {
                SenderId = sender.Id,
                Content = message,
                ReceivedOn = DateTimeOffset.Now
            };

            _chatMessageRepository.Add(chatMessage);
            _chatMessageRepository.Save();

            // So, broadcast the sender, too.
            Clients.All.received(new { sender = senderName, message = message, isPrivate = false });
        }

        public void Send(string message, string to)
        {
            string senderName = Context.User.Identity.Name;
            User sender = _userRepository.GetSingleByUsername(senderName);
            User receiver = _userRepository.GetSingleByUsername(to);

            if (receiver != null)
            {
                IEnumerable<HubConnection> receiverConnections = _hubConnectionRepository
                    .GetAllIncluding(conn => conn.User)
                    .Where(conn => conn.User.Name == to).ToArray();

                PrivateChatMessage privateChatMessage = new PrivateChatMessage
                {
                    ReceiverId = receiver.Id,
                    SenderId = sender.Id,
                    Content = message,
                    ReceivedOn = DateTimeOffset.Now
                };

                _privateChatMessageRepository.Add(privateChatMessage);
                _privateChatMessageRepository.Save();

                if (receiverConnections.Any())
                {
                    IEnumerable<HubConnection> senderConnections = _hubConnectionRepository
                        .GetAllIncluding(conn => conn.User)
                        .Where(conn => conn.User.Name == senderName).ToArray();

                    IEnumerable<string> allReceivers = senderConnections.Select(conn => conn.ConnectionId)
                        .Concat(receiverConnections.Select(conn => conn.ConnectionId));

                    foreach (string connectionId in allReceivers)
                    {
                        Clients.Client(connectionId).received(new { sender = senderName, message = message, isPrivate = true });
                    }
                }
            }
        }

        public IEnumerable<string> GetConnectedUsers()
        {
            string userName = Context.User.Identity.Name;

            // NOTE: Usernames are unique. This query is based on that
            IQueryable<string> connectedUserNameCollection = (from connection in _hubConnectionRepository.GetAllIncluding(conn => conn.User)
                                                              where connection.User.Name != userName
                                                              group connection by connection.UserId into g
                                                              select g.FirstOrDefault().User.Name);

            return connectedUserNameCollection.ToArray();
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            User user = _membershipService.GetUser(userName).User;
            HubConnection hubConnection = new HubConnection
            {
                UserId = user.Id,
                ConnectionId = connectionId
            };

            _hubConnectionRepository.Add(hubConnection);
            _hubConnectionRepository.Save();

            int userConnectionCount = _hubConnectionRepository.GetAll().Count(conn => conn.UserId == user.Id);
            if (userConnectionCount == 1)
            {
                Clients.Others.userConnected(userName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            UserWithRoles userWithRoles = _membershipService.GetUser(userName);
            if(userWithRoles != null)
            {
                HubConnection hubConnection = _hubConnectionRepository.GetAll().FirstOrDefault(conn => conn.ConnectionId == connectionId);
                if (hubConnection != null)
                {
                    _hubConnectionRepository.Delete(hubConnection);
                    _hubConnectionRepository.Save();
                }

                if (!_hubConnectionRepository.GetAll().Any()) 
                {
                    Clients.Others.userDisconnected(userName);
                }
            }

            return base.OnDisconnected();
        }
    }
}