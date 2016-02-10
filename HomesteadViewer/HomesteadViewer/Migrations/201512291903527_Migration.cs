namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdministrativeProperties", "FollowUpDate", c => c.DateTime());
            AddColumn("dbo.AdministrativeProperties", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdministrativeProperties", "Comment");
            DropColumn("dbo.AdministrativeProperties", "FollowUpDate");
        }
    }
}
