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
    internal class menus_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<menus_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("menus");
            entityTypeBuilder.HasKey(q => q.menu_id);
            entityTypeBuilder.Property(q => q.menu_id).IsRequired();
            entityTypeBuilder.Property(q => q.name).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.icon_name).HasMaxLength(100);
            entityTypeBuilder.Property(q => q.description_alt).HasMaxLength(100);
            entityTypeBuilder.Property(q => q.menu_url).HasMaxLength(150);
            entityTypeBuilder.Property(q => q.is_show_navbar).IsRequired();
            entityTypeBuilder.Property(q => q.is_show_home_menu).IsRequired();
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");
        }
    }
}
