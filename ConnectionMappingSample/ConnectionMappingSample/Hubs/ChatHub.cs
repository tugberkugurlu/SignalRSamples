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

    [Authorize]
    public class ChatHub : Hub {

        // References: 
        // https://github.com/SignalR/SignalR/wiki/Hubs
        // https://github.com/SignalR/Samples/blob/master/BasicChat/ChatWithTracking.cs
        // https://github.com/davidfowl/MessengR/blob/master/MessengR/Hubs/Chat.cs

        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();

        public void Send(string message) {

            User sender = GetUser(Context.ConnectionId);
            
            // Obviously, as it says Caller.name!, this sends it to the caller only (obviously!!).
            // Clients.Caller.name = userName;

            // So, broadcast the sender, too.
            Clients.All.received(new { sender = sender.Name, message = message, isPrivate = false });
        }

        public void Send(string message, string to) {

            // Gets the username from the collection by looking at the connection id.
            // We could also get the username through the authed user (Context.User.Identity.Name)
            User sender = GetUser(Context.ConnectionId);
            User receiver = Users.FirstOrDefault(x => x.Key.Equals(to, StringComparison.InvariantCultureIgnoreCase)).Value;

            IEnumerable<string> allReceivers = receiver.ConnectionIds.Concat(sender.ConnectionIds);
            foreach (var cid in allReceivers) {
                Clients.Client(cid).received(new { sender = sender.Name, message = message, isPrivate = true });
            }
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

            // // broadcast this to all clients other than the caller
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

                // // broadcast this to all clients other than the caller
                // Clients.AllExcept(disconnectedUserCids).userDisconnected(userName);
            }

            return base.OnDisconnected();
        }

        // privates

        private User GetUser(string cid) {

            // Gets the user from the collection by looking at the connection id.
            // We could also get the username through the authed user (Context.User.Identity.Name)
            return Users.FirstOrDefault(x =>
                x.Value.ConnectionIds.Contains(
                    cid, StringComparer.InvariantCultureIgnoreCase)).Value;
        }
    }
}