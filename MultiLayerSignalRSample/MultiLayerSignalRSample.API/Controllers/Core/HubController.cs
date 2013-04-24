using System;
using Microsoft.AspNet.SignalR;

namespace MultiLayerSignalRSample.API.Controllers.Core
{
    public abstract class HubController : HubControllerBase
    {
        private readonly string _hubName;

        protected HubController(string hubName)
        {
            if (hubName == null)
            {
                throw new ArgumentNullException("hubName");
            }
            _hubName = hubName;
        }

        protected override IHubContext HubContext
        {
            get
            {
                return ConnectionManager.GetHubContext(_hubName);
            }
        }
    }
}