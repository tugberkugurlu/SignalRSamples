namespace MultiLayerSignalRSample.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        Content = c.String(),
                        ReceivedOn = c.DateTimeOffset(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: true)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.PrivateChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Content = c.String(),
                        ReceivedOn = c.DateTimeOffset(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SenderId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.ReceiverId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.HubConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ConnectionId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HubConnections", "UserId", "dbo.Users");
            DropForeignKey("dbo.PrivateChatMessages", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.PrivateChatMessages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.ChatMessages", "SenderId", "dbo.Users");
            DropIndex("dbo.HubConnections", new[] { "UserId" });
            DropIndex("dbo.PrivateChatMessages", new[] { "ReceiverId" });
            DropIndex("dbo.PrivateChatMessages", new[] { "SenderId" });
            DropIndex("dbo.ChatMessages", new[] { "SenderId" });
            DropTable("dbo.HubConnections");
            DropTable("dbo.PrivateChatMessages");
            DropTable("dbo.ChatMessages");
            DropTable("dbo.Users");
        }
    }
}
