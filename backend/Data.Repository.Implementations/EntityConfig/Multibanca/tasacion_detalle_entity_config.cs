using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class tasacion_detalle_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<tasacion_detalle_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("tasacion_detalle");

            entityTypeBuilder.HasKey(q => q.id_tasacion_detalle);

            entityTypeBuilder.Property(q => q.id_tasacion_detalle).IsRequired();
            entityTypeBuilder.Property(q => q.id_tasacion).IsRequired();
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.tipo_tasacion).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.nro_tasacion_p1).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.nro_tasacion_p2).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.nro_tasacion_p3).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.superficie_edificada).HasMaxLength(200);
            entityTypeBuilder.Property(q => q.superficie_terreno).HasMaxLength(200);
            entityTypeBuilder.Property(q => q.fecha_informe_tasacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_solicitud_tasacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.fecha_recepcion_tasacion).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.valor_tasacion_uf).HasColumnType("numeric(18,4)");
            entityTypeBuilder.Property(q => q.valor_tasacion_pesos).HasColumnType("numeric(18,2)");
            entityTypeBuilder.Property(q => q.valor_liquidacion_uf).HasColumnType("numeric(18,4)");
            entityTypeBuilder.Property(q => q.valor_liquidacion_pesos).HasColumnType("numeric(18,2)");
            entityTypeBuilder.Property(q => q.monto_asegurable_uf).HasColumnType("numeric(18,4)");
            entityTypeBuilder.Property(q => q.monto_asegurable_pesos).HasColumnType("numeric(18,2)");

            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => new { q.id_expediente })
                .HasDatabaseName("ix_tasacion_detalle_expediente");

            entityTypeBuilder.HasIndex(q => new { q.id_tasacion })
                .HasDatabaseName("ix_tasacion_detalle_tasacion");
        }
    }
}
