using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web; 
using System.DirectoryServices;
using HomesteadViewer.Models;
using HomesteadViewer.DAL;
using System.ComponentModel;
using HomesteadViewer.SiteUtilities;

namespace HomesteadViewer.ViewModels.Administration
{
    public class ApplicationSettingsViewModel
    {
        [DisplayName("Fully Qualified Domain Name")]
        [Description("e.x. BIS")]
        public string FullyQualifiedDomainName { get; set; }

        [DisplayName("Data Source")]
        [Description("The location of your SQL Server.")]
        public string DataSource { get; set; }

        [DisplayName("Catalog")]
        [Description("The database, table, or viewname to connect to")]
        public string Catalog { get; set; }

        [DisplayName("File Repository Path")]
        [Description("The database, table, or viewname to connect to")]
        public string FileRepositoryPath { get; set; }

        [DisplayName("Search Term Table Column")]
        [Description("The database column used (as a key) to search for files by the file name.")]
        public string SearchTermTableColumn { get; set; }

        [DisplayName("Site Title")]
        public string SiteTitle { get; set; }

        [DisplayName("Anyone Can Register?")]
        [Description("If set to yes, when the user logs into the site with their domain account, an account will be created for them on this application.")]
        public bool AnyoneCanRegister { get; set; }

        [DisplayName("Approval For Access?")]
        [Description("Do new users need to be approved before they can have access?")]
        public bool ApprovalForAccess { get; set; }

        [DisplayName("Only Admins Can Assign Exemptions")]
        [Description("If this is not checked, all users will be able to assign Exemptions to application users.")]
        public bool OnlyAdminCanAssign { get; set; }

        public ApplicationSettingsViewModel()
        {
            this.AnyoneCanRegister = AppSettings.AnyoneCanRegister;
            this.ApprovalForAccess = AppSettings.ApprovalForAccess;
            this.Catalog = AppSettings.Catalog;
            this.FullyQualifiedDomainName = AppSettings.FullyQualifiedDomainName;
            this.DataSource = AppSettings.DataSource;
            this.FileRepositoryPath = AppSettings.FileRepositoryPath;
            this.OnlyAdminCanAssign = AppSettings.OnlyAdminCanAssign;
            this.SearchTermTableColumn = AppSettings.SearchTermTableColumn;
            this.SiteTitle = AppSettings.SiteTitle;
        }
    }
 
    public class GridSettingsViewModel : HomesteadViewer.ViewModels.Exemption
    {
    
    }
}