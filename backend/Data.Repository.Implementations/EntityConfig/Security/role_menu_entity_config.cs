using Data.Repository.Interfaces.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.EntityConfig.Security
{
    internal class role_menu_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<role_menu_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("role_menu");
            entityTypeBuilder.HasKey(q => q.role_menu_id);
            entityTypeBuilder.Property(q => q.role_menu_id).IsRequired();
            entityTypeBuilder.Property(q => q.role_id).IsRequired();
            entityTypeBuilder.Property(q => q.menu_id).IsRequired();
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasOne(q => q.role).WithMany(q => q.role_menus).HasForeignKey(q => q.role_id);
            entityTypeBuilder.HasOne(q => q.menu).WithMany(q => q.role_menus).HasForeignKey(q => q.menu_id);
        }
    }
}
