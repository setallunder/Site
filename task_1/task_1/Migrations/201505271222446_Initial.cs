namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Template = c.String(),
                        Url = c.String(),
                        Rating = c.Int(nullable: false),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.String(),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profiles", t => t.Profile_Id, cascadeDelete: true)
                .Index(t => t.Profile_Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessCardJoinFields",
                c => new
                    {
                        BusinessCardId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusinessCardId, t.FieldId })
                .ForeignKey("dbo.BusinessCards", t => t.BusinessCardId, cascadeDelete: true)
                .ForeignKey("dbo.Fields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.BusinessCardId)
                .Index(t => t.FieldId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fields", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.BusinessCards", "Profile_Id", "dbo.Profiles");
            DropForeignKey("dbo.BusinessCardJoinFields", "FieldId", "dbo.Fields");
            DropForeignKey("dbo.BusinessCardJoinFields", "BusinessCardId", "dbo.BusinessCards");
            DropIndex("dbo.BusinessCardJoinFields", new[] { "FieldId" });
            DropIndex("dbo.BusinessCardJoinFields", new[] { "BusinessCardId" });
            DropIndex("dbo.Fields", new[] { "Profile_Id" });
            DropIndex("dbo.BusinessCards", new[] { "Profile_Id" });
            DropTable("dbo.BusinessCardJoinFields");
            DropTable("dbo.Profiles");
            DropTable("dbo.Fields");
            DropTable("dbo.BusinessCards");
        }
    }
}
