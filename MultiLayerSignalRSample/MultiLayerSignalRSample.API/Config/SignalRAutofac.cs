using Microsoft.AspNet.SignalR;
using Autofac;
using MultiLayerSignalRSample.Domain.Entities.Core;
using Autofac.Integration.SignalR;
using MultiLayerSignalRSample.Domain.Services;
using System.Reflection;

namespace MultiLayerSignalRSample.API.Config
{
    public static class SignalRAutofac
    {
        public static void Initialize()
        {
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(RegisterServices(new ContainerBuilder()));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterType<ChatEntitiesContext>().As<IEntitiesContext>();
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>));
            builder.RegisterType<MembershipService>().As<IMembershipService>();

            return builder.Build();
        }
    }
}