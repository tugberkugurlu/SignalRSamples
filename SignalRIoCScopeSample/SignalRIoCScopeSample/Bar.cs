using System;

namespace SignalRIoCScopeSample
{
    public class Bar : IBar
    {
        private readonly IBroadcaster _broadcaster;

        public Bar(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public DateTimeOffset Broadcast()
        {
            return _broadcaster.Broadcast();
        }
    }

    public interface IBar
    {
        DateTimeOffset Broadcast();
    }
}