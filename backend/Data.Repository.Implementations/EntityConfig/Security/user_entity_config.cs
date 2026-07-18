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
    internal class user_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<users_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("users");
            entityTypeBuilder.HasKey(q => q.user_id);
            entityTypeBuilder.Property(q => q.user_id).IsRequired();
            entityTypeBuilder.Property(q => q.role_id).IsRequired();
            entityTypeBuilder.Property(q => q.name).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(q => q.last_name_first).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(q => q.last_name_second).HasMaxLength(100);
            entityTypeBuilder.Property(q => q.id_document_type);
            entityTypeBuilder.Property(q => q.nro_document).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.user_name).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(q => q.password).HasMaxLength(500).IsRequired();
            entityTypeBuilder.Property(q => q.email).HasMaxLength(100);
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");
            entityTypeBuilder.HasOne(q => q.role).WithMany(q => q.users).HasForeignKey(q => q.role_id);
        }
    }
}
