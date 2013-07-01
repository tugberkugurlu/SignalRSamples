using Microsoft.AspNet.SignalR;
using Owin;

namespace RedisScaleOutSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseRedis("localhost", 6379, string.Empty, "myApp");
            app.MapHubs();
        }
    }
}