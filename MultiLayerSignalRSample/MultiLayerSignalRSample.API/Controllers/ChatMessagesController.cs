using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiLayerSignalRSample.API.Controllers.Core;
using MultiLayerSignalRSample.API.Hubs;
using MultiLayerSignalRSample.Domain.Entities;
using MultiLayerSignalRSample.Domain.Entities.Core;

namespace MultiLayerSignalRSample.API.Controllers
{
    public class ChatMessagesController : HubController<ChatHub>
    {
        private readonly IEntityRepository<ChatMessage> _chatMessageRepository;

        public ChatMessagesController(IEntityRepository<ChatMessage> chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
    }
}