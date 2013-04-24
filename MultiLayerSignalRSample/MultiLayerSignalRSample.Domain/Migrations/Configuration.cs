namespace MultiLayerSignalRSample.Domain.Migrations
{
    using MultiLayerSignalRSample.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MultiLayerSignalRSample.Domain.Entities.Core.ChatEntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MultiLayerSignalRSample.Domain.Entities.Core.ChatEntitiesContext context)
        {
            context.Roles.AddOrUpdate(
                role => role.Id,
                new Role 
                { 
                    Id = 1,
                    Name = RoleConstants.CHAT_USER_ROLE_NAME
                });
        }
    }
}