using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ConnectionMappingSample.Hubs {

    public class User {

        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    public class Room {

        public string RoomName { get; set; }
        public HashSet<string> Users { get; set; }
    }

    [Authorize]
    public class ChatHub : Hub {

        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        public void Send(string message) {

            // Gets the username from the collection by looking at the connection id.
            // We might also get the username through the authed user
            string userName = Users.FirstOrDefault(x =>
                x.Value.ConnectionIds.Contains(
                    Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase)).Key;

            // Obviously, this sends it to the caller only (obviously!!).
            // Clients.Caller.name = userName;
            Clients.All.Received(new { sender = userName, message = message });
        }

        public IEnumerable<string> GetConnectedUsers() {

            return Users.Where(x => !x.Value.ConnectionIds.Contains(
                Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase)).Select(x => x.Key);
        }

        public override Task OnConnected() {

            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            User user = Users.Values.FirstOrDefault(x => x.Name.Equals(
                Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));

            if (user == null) {

                user = new User { 
                    Name = userName, 
                    ConnectionIds = new HashSet<string>() 
                };
            }

            user.ConnectionIds.Add(connectionId);
            Users.AddOrUpdate(userName, user, (n, u) => user);

            // // broadcast this to all clients other that the caller
            // Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);

            // Or you might want to only broadcast this info if this 
            // is the first connection of the user
            if (user.ConnectionIds.Count == 1) {

                Clients.Others.userConnected(userName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected() {

            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            User user = Users.Values.FirstOrDefault(x => x.Name.Equals(
                Context.User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
            
            string[] disconnectedUserCids;
            if (user != null) {

                user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                disconnectedUserCids = user.ConnectionIds.ToArray();

                if (!user.ConnectionIds.Any()) {

                    User removedUser;
                    Users.TryRemove(userName, out removedUser);

                    // Or you might want to only broadcast this info if this 
                    // is the last connection of the user and the user actual is now disconnected
                    // from all connections.
                    Clients.Others.userDisconnected(userName);
                }
                else {

                    Users.AddOrUpdate(userName, user, (n, u) => user);
                }

                // // broadcast this to all clients other that the caller
                // Clients.AllExcept(disconnectedUserCids).userDisconnected(userName);
            }

            return base.OnDisconnected();
        }
    }
}