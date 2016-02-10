using HomesteadViewer.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HomesteadViewer.DAL
{
    public class QuedExemptionsContext : DbContext
    {
        public QuedExemptionsContext()
            : base("ExemptionEntriesContext")
        {
            Database.SetInitializer<QuedExemptionsContext>(null);
        }

        public DbSet<QuedExemption> QuedExemptions { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}