namespace MultiLayerSignalRSample.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedUserManagement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserInRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 320));
            AddColumn("dbo.Users", "HashedPassword", c => c.String(nullable: false));
            AddColumn("dbo.Users", "Salt", c => c.String(nullable: false));
            AddColumn("dbo.Users", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "CreatedOn", c => c.DateTimeOffset(nullable: false));
            AddColumn("dbo.Users", "LastUpdatedOn", c => c.DateTimeOffset(nullable: false));
            DropColumn("dbo.Users", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserName", c => c.String());
            DropForeignKey("dbo.UserInRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserInRoles", "UserId", "dbo.Users");
            DropIndex("dbo.UserInRoles", new[] { "RoleId" });
            DropIndex("dbo.UserInRoles", new[] { "UserId" });
            DropColumn("dbo.Users", "LastUpdatedOn");
            DropColumn("dbo.Users", "CreatedOn");
            DropColumn("dbo.Users", "IsLocked");
            DropColumn("dbo.Users", "Salt");
            DropColumn("dbo.Users", "HashedPassword");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Name");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
        }
    }
}
