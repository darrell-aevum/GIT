namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdministrativeProperties", "AssignedUserID", "dbo.Users");
            DropIndex("dbo.AdministrativeProperties", new[] { "AssignedUserID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.AdministrativeProperties", "AssignedUserID");
            AddForeignKey("dbo.AdministrativeProperties", "AssignedUserID", "dbo.Users", "Id");
        }
    }
}
