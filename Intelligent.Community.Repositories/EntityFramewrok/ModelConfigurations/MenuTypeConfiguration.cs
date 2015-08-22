using Intelligent.Community.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Intelligent.Community.Domain.Repositories.EntityFramewrok.ModelConfigurations
{
    public class MenuTypeConfiguration : EntityTypeConfiguration<Menu>
    {
        public MenuTypeConfiguration()
        {
            HasKey<Guid>(m => m.ID);
            Property(m => m.ID)
               .IsRequired()
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(20);

            Property(m => m.Code)
                .HasMaxLength(20);

            Property(m => m.IdParent)
               .IsRequired();

            Property(m => m.LinkPath)
               .HasMaxLength(500);


            ToTable("ms_Menu");
        }
    }
}
