using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MultiLayerSignalRSample.API.Config;

namespace MultiLayerSignalRSample {

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {

        protected void Application_Start() {
            
            // Web API Config
            HttpConfiguration httpConfiguration = GlobalConfiguration.Configuration;
            WebApiAutofac.Initialize(httpConfiguration);
            WebApiRouteConfig.RegisterRoutes(httpConfiguration);
            WebApiConfig.Configure(httpConfiguration);

            // SignalR Config
            SignalRAutofac.Initialize();
            RouteTable.Routes.MapHubs();

            // MVC Config
            AutofacMvc.Initialize();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}