namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Message = c.String(),
                        LineNumber = c.Int(),
                        MethodName = c.String(),
                        Classname = c.String(),
                        AdditionalIfno = c.String(),
                        InnerException = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ErrorLogs");
        }
    }
}
