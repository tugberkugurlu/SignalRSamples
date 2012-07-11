using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace MoveShapeDemo.Hubs {

    [HubName("MoveShape")]
    public class MoveShapeHub : Hub {

        public void MoveShape(int x, int y) {

            Clients.shapeMoved(Context.ConnectionId, x, y);
        }
    }
}