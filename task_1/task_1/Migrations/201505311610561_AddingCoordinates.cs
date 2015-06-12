namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCoordinates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessCardToFields", "OffsetX", c => c.Int(nullable: false));
            AddColumn("dbo.BusinessCardToFields", "OffsetY", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessCardToFields", "OffsetY");
            DropColumn("dbo.BusinessCardToFields", "OffsetX");
        }
    }
}
