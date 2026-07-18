using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class rectificatoria_analisis_derivacion_reparo_postventa_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<rectificatoria_analisis_derivacion_reparo_postventa_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("rectificatoria_analisis_derivacion_reparo_postventa");

            entityTypeBuilder.HasKey(q => q.id_rectificatoria_analisis_derivacion_reparo_postventa);

            entityTypeBuilder.Property(q => q.id_rectificatoria_analisis_derivacion_reparo_postventa).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.enviar_reparo_a).HasColumnName("enviar_reparo_a");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_rectificatoria_analisis_deivacion_reparo_postventa_expediente");
        }
    }
}
