namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdministrativeProperties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OnlineExemptionID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AssignedUserID = c.Int(),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.AssignedUserID)
                .Index(t => t.AssignedUserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        EmailAddress = c.String(nullable: false, maxLength: 250),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Department = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        Phone = c.String(maxLength: 12),
                        MobileNumber = c.String(maxLength: 12),
                        JobTitle = c.String(maxLength: 50),
                        Extension = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.GridColumns",
                c => new
                    {
                        PropertyName = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(),
                        ColumnNumber = c.Int(nullable: false),
                        Displayed = c.Boolean(nullable: false),
                        Editable = c.Boolean(nullable: false),
                        Width = c.Int(),
                    })
                .PrimaryKey(t => t.PropertyName);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingGroup = c.String(nullable: false, maxLength: 50),
                        SettingKey = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(),
                    })
                .PrimaryKey(t => new { t.SettingGroup, t.SettingKey });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdministrativeProperties", "AssignedUserID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.AdministrativeProperties", new[] { "AssignedUserID" });
            DropTable("dbo.Settings");
            DropTable("dbo.GridColumns");
            DropTable("dbo.Users");
            DropTable("dbo.AdministrativeProperties");
        }
    }
}
