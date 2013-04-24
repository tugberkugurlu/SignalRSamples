using System.Reflection;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Autofac;
using MultiLayerSignalRSample.Domain.Entities.Core;
using MultiLayerSignalRSample.Domain.Services;

namespace MultiLayerSignalRSample.API.Config
{
    public static class WebApiAutofac
    {
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ChatEntitiesContext>().As<IEntitiesContext>();
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>));
            builder.RegisterType<MembershipService>().As<IMembershipService>();

            return builder.Build();
        }
    }
}
