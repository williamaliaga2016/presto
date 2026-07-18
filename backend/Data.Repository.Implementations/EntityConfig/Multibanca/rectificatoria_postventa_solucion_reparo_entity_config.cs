using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class rectificatoria_postventa_solucion_reparo_entity_config
    {
        public static void SetEntityBuilder(
            EntityTypeBuilder<rectificatoria_postventa_solucion_reparo_entity> entityTypeBuilder
        )
        {
            entityTypeBuilder.ToTable("rectificatoria_postventa_solucion_reparo");

            entityTypeBuilder.HasKey(q => q.id_rectificatoria_postventa_solucion_reparo);

            entityTypeBuilder.Property(q => q.id_rectificatoria_postventa_solucion_reparo).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.id_usuario_solicitante).IsRequired();
            entityTypeBuilder.Property(q => q.is_subsanar).IsRequired().HasDefaultValue(false);
            entityTypeBuilder.Property(q => q.modificar_datos_memo).IsRequired();
            entityTypeBuilder.Property(q => q.descontabilizar_operacion).IsRequired();
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date)
                .IsRequired()
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_rectificatoria_postventa_solucion_reparo_expediente");
        }
    }
}
