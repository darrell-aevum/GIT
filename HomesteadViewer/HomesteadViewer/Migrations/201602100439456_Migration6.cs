namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration6 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AdministrativeProperties", "OnlineExemptionID", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AdministrativeProperties", new[] { "OnlineExemptionID" });
        }
    }
}
