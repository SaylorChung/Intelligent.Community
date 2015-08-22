using Intelligent.Community.Domain.Models;
using Intelligent.Community.Domain.Repositories.EntityFramewrok.ModelConfigurations;
using Saylor.Repositories.EntityFramework;
using System.Data.Entity;

namespace Intelligent.Community.Domain.Repositories.EntityFramewrok
{
    public partial class CommunityDbContext : DatabaseContext
    {
        #region Ctor
        public CommunityDbContext()
            : base("CommunityDbContext")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }
        #endregion

        #region Public Properties
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Menu> Menus { get; set; }

        public DbSet<Car> Cars { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations
                .Add(new EmployeeTypeConfiguration())
                .Add(new MenuTypeConfiguration())
                
                .Add(new CarTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
