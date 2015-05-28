namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingHandMadeManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BusinessCardJoinFields", "BusinessCardId", "dbo.BusinessCards");
            DropForeignKey("dbo.BusinessCardJoinFields", "FieldId", "dbo.Fields");
            DropIndex("dbo.BusinessCardJoinFields", new[] { "BusinessCardId" });
            DropIndex("dbo.BusinessCardJoinFields", new[] { "FieldId" });
            CreateTable(
                "dbo.BusinessCardToFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessCardId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Fields", "BusinessCard_Id", c => c.Int());
            CreateIndex("dbo.Fields", "BusinessCard_Id");
            AddForeignKey("dbo.Fields", "BusinessCard_Id", "dbo.BusinessCards", "Id");
            DropTable("dbo.BusinessCardJoinFields");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BusinessCardJoinFields",
                c => new
                    {
                        BusinessCardId = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusinessCardId, t.FieldId });
            
            DropForeignKey("dbo.Fields", "BusinessCard_Id", "dbo.BusinessCards");
            DropIndex("dbo.Fields", new[] { "BusinessCard_Id" });
            DropColumn("dbo.Fields", "BusinessCard_Id");
            DropTable("dbo.BusinessCardToFields");
            CreateIndex("dbo.BusinessCardJoinFields", "FieldId");
            CreateIndex("dbo.BusinessCardJoinFields", "BusinessCardId");
            AddForeignKey("dbo.BusinessCardJoinFields", "FieldId", "dbo.Fields", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BusinessCardJoinFields", "BusinessCardId", "dbo.BusinessCards", "Id", cascadeDelete: true);
        }
    }
}
