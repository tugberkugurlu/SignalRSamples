using Autofac;
using Microsoft.AspNet.SignalR;

namespace SignalRIoCScopeSample
{
    public class PingHub : Hub
    {
        private readonly IBar _bar;
        private readonly IFoo _foo;
        private readonly ILifetimeScope _hubLifetimeScope;

        public PingHub(ILifetimeScope lifetimeScope)
        {
            // Create a lifetime scope for this hub instance.
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

            // Resolve the dependencies from the hub lifetime scope (unfortunately, service locator style).
            _bar = _hubLifetimeScope.Resolve<IBar>();
            _foo = _hubLifetimeScope.Resolve<IFoo>();
        }

        public void Ping()
        {
            Clients.All.pinged("Pinged for IBar: " + _bar.Broadcast().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            Clients.All.pinged("Pinged for IFoo: " + _foo.Broadcast().ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
        }

        protected override void Dispose(bool disposing)
        {
            // When SignalR disposes the hub clean up the lifetime scope.
            if (disposing && _hubLifetimeScope != null)
            {
                _hubLifetimeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}