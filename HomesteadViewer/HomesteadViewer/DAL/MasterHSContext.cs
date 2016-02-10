using HomesteadViewer.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
namespace HomesteadViewer.DAL
{
    public class MasterHSContext : DbContext
    {
        public MasterHSContext()
            : base("MasterHSContext")
        {
            Database.SetInitializer<MasterHSContext>(null);
            this.Database.Connection.ConnectionString = AppSettings.ConnectionString;
            try
            {
                this.Database.Connection.Open();
            }
            catch(Exception ex)
            {
                throw new Exception("Could not connect to database:<br/>" + AppSettings.DataSource + " (" + AppSettings.Catalog + ")");
            }

        }

        public DbSet<MasterExemption_DTO> MasterExemptions { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}