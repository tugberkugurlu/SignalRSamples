using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace SignalRGroupSample.Hubs {

    public class Chat : Hub {

        public void Send(string data) {

            Clients.addMessage(data, this.Context.ConnectionId);
        }

        public void Send(string data, string room) {

            Clients[room].addMessage(data, this.Context.ConnectionId.ToString() + "on room: " + room);
        }

        public void JoinRoom(string room) {

            AddToGroup(room);
            Clients[room].announceNewcommer(this.Context.ConnectionId);
        }
    }
}