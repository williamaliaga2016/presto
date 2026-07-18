using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class registrar_fecha_registro_cbr_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<registrar_fecha_registro_cbr_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("registrar_fecha_registro_cbr");

            entityTypeBuilder.HasKey(q => q.id_registrar_fecha_registro_cbr);

            entityTypeBuilder.Property(q => q.id_registrar_fecha_registro_cbr).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.fecha_registro_cbr).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(4000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_registrar_fecha_registro_cbr_expediente");
        }
    }
}
