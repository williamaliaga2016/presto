using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class tasacion_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<tasacion_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("tasacion");

            entityTypeBuilder.HasKey(q => q.id_tasacion);

            entityTypeBuilder.Property(q => q.id_tasacion).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.is_enviar_reparo).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(2000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_tasacion_expediente");
        }
    }
}
