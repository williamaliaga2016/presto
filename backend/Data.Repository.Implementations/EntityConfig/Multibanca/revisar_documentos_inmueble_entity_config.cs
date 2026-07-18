using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class revisar_documentos_inmueble_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<revisar_documentos_inmueble_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("revisar_documentos_inmueble");
            entityTypeBuilder.HasKey(q => q.id);
            entityTypeBuilder.Property(q => q.id).ValueGeneratedOnAdd();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_actividad).IsRequired().HasMaxLength(100);
            entityTypeBuilder.Property(q => q.documentos_correctos);
            entityTypeBuilder.Property(q => q.motivo_devolucion).HasMaxLength(100);
            entityTypeBuilder.Property(q => q.observaciones).HasColumnType("text");
            entityTypeBuilder.Property(q => q.requiere_actualizacion_avaluos).HasMaxLength(10);
            entityTypeBuilder.Property(q => q.homologacion).HasMaxLength(10);
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");
            entityTypeBuilder.HasIndex(q => q.id_expediente).HasDatabaseName("idx_rdi_expediente");
        }
    }
}
