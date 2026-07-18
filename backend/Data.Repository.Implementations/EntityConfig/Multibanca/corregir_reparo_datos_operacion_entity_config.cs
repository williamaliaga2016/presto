using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class corregir_reparo_datos_operacion_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<corregir_reparo_datos_operacion_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("corregir_reparo_datos_operacion");

            entityTypeBuilder.HasKey(q => q.id_corregir_reparo_datos_operacion);

            entityTypeBuilder.Property(q => q.id_corregir_reparo_datos_operacion).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.is_subsanar).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_corregir_reparo_datos_operacion_expediente");
        }
    }
}
