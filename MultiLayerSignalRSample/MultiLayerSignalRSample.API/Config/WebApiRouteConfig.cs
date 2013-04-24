using System.Web.Http;

namespace MultiLayerSignalRSample.API.Config
{
    public static class WebApiRouteConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            HttpRouteCollection routes = config.Routes;

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
