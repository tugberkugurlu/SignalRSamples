using Microsoft.AspNet.SignalR;

namespace SignalRIoCScopeSample
{
    public class PingHub : Hub
    {
        private readonly IBar _bar;
        private readonly IFoo _foo;
        private readonly IBroadcaster _broadcaster;

        public PingHub(IBar bar, IFoo foo)
        {
            _bar = bar;
            _foo = foo;
        }

        public void Ping()
        {
            Clients.All.pinged("Pinged for IBar: " + _bar.Broadcast().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            Clients.All.pinged("Pinged for IFoo: " + _foo.Broadcast().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
        }
    }
}