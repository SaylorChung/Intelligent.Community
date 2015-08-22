using Intelligent.Community.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace Intelligent.Community.Domain.Repositories.EntityFramewrok.ModelConfigurations
{
    public class EmployeeTypeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeTypeConfiguration()
        {
            HasKey<Guid>(m => m.ID);
            Property(m => m.ID)
               .IsRequired()
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(20);

            Property(m => m.Password)
                .IsRequired()
                .HasMaxLength(100);

            Property(m => m.RealName)
               .IsRequired()
               .HasMaxLength(20);

            Property(m => m.Email)
               .HasMaxLength(100);

            Property(m => m.Mobile)
               .HasMaxLength(100);

            Property(m => m.Status)
               .IsRequired();

            Property(m => m.Memo)
               .HasMaxLength(500);


            HasMany(a => a.Menus)
               .WithMany(c => c.Employees)
               .Map(x => x.MapLeftKey("EmployeeID")
               .MapRightKey("MenuID").ToTable("ms_Employee_Menu"));

            ToTable("ms_Employee");
        }
    }
}
