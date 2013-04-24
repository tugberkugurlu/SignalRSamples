using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MultiLayerSignalRSample.API.Controllers.Core
{
    public abstract class HubControllerOfTHub<THub> : HubControllerBase where THub : IHub
    {
        protected override IHubContext HubContext
        {
            get
            {
                return ConnectionManager.GetHubContext<THub>();
            }
        }
    }
}
