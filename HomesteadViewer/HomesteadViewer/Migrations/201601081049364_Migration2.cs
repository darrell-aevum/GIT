namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTimeLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstLogDate = c.DateTime(nullable: false),
                        LastLogDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AdministrativeProperties", "DateAssigned", c => c.DateTime());
            AddColumn("dbo.AdministrativeProperties", "DateStatusChanged", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdministrativeProperties", "DateStatusChanged");
            DropColumn("dbo.AdministrativeProperties", "DateAssigned");
            DropTable("dbo.UserTimeLogs");
        }
    }
}
