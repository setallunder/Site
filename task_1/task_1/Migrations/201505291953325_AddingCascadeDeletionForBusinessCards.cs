namespace task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCascadeDeletionForBusinessCards : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BusinessCards", "Profile_Id", "dbo.Profiles");
            AddForeignKey("dbo.BusinessCards", "Profile_Id", "dbo.Profiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessCards", "Profile_Id", "dbo.Profiles");
            AddForeignKey("dbo.BusinessCards", "Profile_Id", "dbo.Profiles", "Id");
        }
    }
}
