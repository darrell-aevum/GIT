namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exemptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QuedExemptionId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        QuickRefID = c.String(),
                        Driver_License = c.Int(),
                        Birth_Date = c.DateTime(),
                        SpouseBirthDate = c.DateTime(),
                        DateFirstOccupied = c.DateTime(),
                        OwnsProperty = c.String(),
                        Transfer_Tax = c.String(),
                        DeedDate = c.DateTime(),
                        SellerName = c.String(),
                        ExemptionYear = c.String(),
                        ESignature = c.String(),
                        OwnerName = c.String(),
                        PartyName = c.String(),
                        HS_CODE = c.String(),
                        HS_Eff_Date = c.DateTime(),
                        HS_ETR_YR = c.String(),
                        OA_Code = c.String(),
                        OA_Eff_Date = c.DateTime(),
                        OA_ETR_YR = c.String(),
                        OAS_ETR_YR = c.String(),
                        DP_Code = c.String(),
                        DP_Eff_Date = c.DateTime(),
                        DP_ETR_YR = c.String(),
                        DV_Code = c.String(),
                        DV_Eff_Date = c.DateTime(),
                        DV_ETR_YR = c.String(),
                        LegalDescription = c.String(),
                        SitusAddress = c.String(),
                        Street_chng = c.String(),
                        City_chng = c.String(),
                        State_chng = c.String(),
                        Zip_chng = c.String(),
                        MailingAddress = c.String(),
                        OwnerAddress1 = c.String(),
                        OwnerAddress2 = c.String(),
                        OwnerCity = c.String(),
                        OwnerState = c.String(),
                        OwnerZip = c.String(),
                        OwnerPhone = c.String(),
                        OwnerEmail = c.String(),
                        fPercentComplete = c.String(),
                        FirstOffImproved = c.String(),
                        FirstOfcama_TSGLand_fHomesite = c.String(),
                        FirstOfcama_TSGImp_fHomesite = c.String(),
                        FollowUpDate = c.DateTime(),
                        Completed = c.String(),
                        Comment = c.String(),
                        Search = c.String(),
                        Assign = c.String(),
                        Additional = c.String(),
                        DocumentName = c.String(),
                        DocumentTypeCode = c.String(),
                        TimestampCreate = c.DateTime(),
                        Date_Added = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                        AvatarImageFileData = c.Binary(),
                        Phone = c.String(maxLength: 12),
                        MobileNumber = c.String(maxLength: 12),
                        JobTitle = c.String(maxLength: 50),
                        Extension = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingGroup = c.String(nullable: false, maxLength: 50),
                        SettingKey = c.String(nullable: false, maxLength: 50),
                        SettingValue = c.String(),
                    })
                .PrimaryKey(t => new { t.SettingGroup, t.SettingKey });
            
            CreateTable(
                "dbo.UserExemption",
                c => new
                    {
                        UserRefId = c.Int(nullable: false),
                        ExemptionRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRefId, t.ExemptionRefId })
                .ForeignKey("dbo.Users", t => t.UserRefId, cascadeDelete: true)
                .ForeignKey("dbo.Exemptions", t => t.ExemptionRefId, cascadeDelete: true)
                .Index(t => t.UserRefId)
                .Index(t => t.ExemptionRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserExemption", "ExemptionRefId", "dbo.Exemptions");
            DropForeignKey("dbo.UserExemption", "UserRefId", "dbo.Users");
            DropIndex("dbo.UserExemption", new[] { "ExemptionRefId" });
            DropIndex("dbo.UserExemption", new[] { "UserRefId" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropTable("dbo.UserExemption");
            DropTable("dbo.Settings");
            DropTable("dbo.Users");
            DropTable("dbo.Exemptions");
        }
    }
}
