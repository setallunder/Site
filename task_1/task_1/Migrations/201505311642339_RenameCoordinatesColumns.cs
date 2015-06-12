namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCoordinatesColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessCardToFields", "OffsetTop", c => c.Int(nullable: false));
            AddColumn("dbo.BusinessCardToFields", "OffsetLeft", c => c.Int(nullable: false));
            DropColumn("dbo.BusinessCardToFields", "OffsetX");
            DropColumn("dbo.BusinessCardToFields", "OffsetY");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusinessCardToFields", "OffsetY", c => c.Int(nullable: false));
            AddColumn("dbo.BusinessCardToFields", "OffsetX", c => c.Int(nullable: false));
            DropColumn("dbo.BusinessCardToFields", "OffsetLeft");
            DropColumn("dbo.BusinessCardToFields", "OffsetTop");
        }
    }
}
