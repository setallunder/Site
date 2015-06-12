namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCoordinatesColumnsToField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fields", "OffsetTop", c => c.Int(nullable: false));
            AddColumn("dbo.Fields", "OffsetLeft", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fields", "OffsetLeft");
            DropColumn("dbo.Fields", "OffsetTop");
        }
    }
}
