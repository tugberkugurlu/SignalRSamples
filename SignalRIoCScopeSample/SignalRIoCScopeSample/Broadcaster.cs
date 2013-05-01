using System;
using System.Diagnostics;

namespace SignalRIoCScopeSample
{
    public class Broadcaster : IBroadcaster
    {
        private readonly DateTimeOffset _date;

        public Broadcaster()
        {
            Trace.TraceInformation("Constructing Broadcaster...");
            _date = DateTimeOffset.Now;
        }

        public DateTimeOffset Broadcast()
        {
            return _date;
        }

        public void Dispose()
        {
            Trace.TraceInformation("Disposing Broadcaster...");
        }
    }

    public interface IBroadcaster : IDisposable
    {
        DateTimeOffset Broadcast();
    }
}