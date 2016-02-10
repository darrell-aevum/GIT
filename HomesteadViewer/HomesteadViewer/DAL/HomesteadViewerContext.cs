using HomesteadViewer.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using HomesteadViewer.Migrations;

namespace HomesteadViewer.DAL
{
    public class HomesteadViewerContext : DbContext
    {
        public HomesteadViewerContext()
            : base("HomesteadViewerContext")
        {
        }

        static HomesteadViewerContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HomesteadViewerContext, Configuration>()); 
        }

        public DbSet<User_DTO> Users { get; set; }
        public DbSet<AdministrativeProperties_DTO> AdministrativeProperties_Collection { get; set; }
        
        public DbSet<Setting_DTO> Settings { get; set; }
        public DbSet<GridColumn_DTO> GridColumns { get; set; }
        public DbSet<ErrorLog_DTO> ErrorLogs { get; set; }
        public DbSet<UserTimeLog_DTO> UserLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}