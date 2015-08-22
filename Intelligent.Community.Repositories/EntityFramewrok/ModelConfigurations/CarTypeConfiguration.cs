using Intelligent.Community.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Intelligent.Community.Domain.Repositories.EntityFramewrok.ModelConfigurations
{
    public class CarTypeConfiguration : EntityTypeConfiguration<Car>
    {
        public CarTypeConfiguration()
        {
            HasKey<Guid>(m => m.ID);
            Property(m => m.ID)
               .IsRequired()
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(m => m.CarCode)
                .IsRequired()
                .HasMaxLength(50);

            Property(m => m.CarNum)
               .HasMaxLength(50);

            Property(m => m.Memo)
               .HasMaxLength(500);

            ToTable("bi_Car");
        }
    }
}
