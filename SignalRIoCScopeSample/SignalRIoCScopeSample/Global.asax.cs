﻿using System;
using System.Reflection;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SignalRIoCScopeSample
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();
            IContainer container = RegisterServices(new ContainerBuilder());
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // The hub will create a lifetime scope manually in its constructor to resolve the IFoo and IBar services.
            builder.RegisterType<Broadcaster>().As<IBroadcaster>().InstancePerLifetimeScope();
            builder.RegisterType<Foo>().As<IFoo>().InstancePerLifetimeScope();
            builder.RegisterType<Bar>().As<IBar>().InstancePerLifetimeScope();

            builder.Register(c => serializer).As<JsonSerializer>();
            return builder.Build();
        }
    }
}