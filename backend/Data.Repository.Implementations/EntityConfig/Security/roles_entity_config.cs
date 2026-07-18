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
    internal class roles_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<roles_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("role");
            entityTypeBuilder.HasKey(q => q.role_id);
            entityTypeBuilder.Property(q => q.role_id).IsRequired();
            entityTypeBuilder.Property(q => q.code).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.name).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");
        }
    }
}
