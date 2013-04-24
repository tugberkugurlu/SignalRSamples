using Autofac;
using Autofac.Integration.Mvc;
using MultiLayerSignalRSample.Domain.Entities.Core;
using MultiLayerSignalRSample.Domain.Services;
using System.Web.Mvc;

namespace MultiLayerSignalRSample
{
    internal static class AutofacMvc
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(RegisterServices(builder)));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<ChatEntitiesContext>().As<IEntitiesContext>();
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>));
            builder.RegisterType<MembershipService>().As<IMembershipService>();

            return builder.Build();
        }
    }
}