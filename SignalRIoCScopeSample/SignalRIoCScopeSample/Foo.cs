using System;

namespace SignalRIoCScopeSample
{
    public class Foo : IFoo
    {
        private readonly IBroadcaster _broadcaster;

        public Foo(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public DateTimeOffset Broadcast()
        {
            return _broadcaster.Broadcast();
        }
    }

    public interface IFoo
    {
        DateTimeOffset Broadcast();
    }
}