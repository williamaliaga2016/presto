using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class CalculoGeneracionDocumentoEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<calculo_generacion_documento_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("calculo_generacion_documento");
            entityTypeBuilder.HasKey(q => q.id_calculo_generacion_documento);
            entityTypeBuilder.Property(q => q.id_expediente).IsRequired();
            entityTypeBuilder.Property(q => q.revision_rol_propiedad).HasMaxLength(50);
            entityTypeBuilder.Property(q => q.valor_uf_fecha_hoy).HasColumnType("numeric(12,2)");
            entityTypeBuilder.Property(q => q.fecha_calculo).HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.valor_uf_fecha_calculo).HasColumnType("numeric(12,2)");
            entityTypeBuilder.Property(q => q.observaciones).HasMaxLength(1000);
            entityTypeBuilder.Property(q => q.is_active).IsRequired();
            entityTypeBuilder.Property(q => q.row_status).IsRequired();
            entityTypeBuilder.Property(q => q.created_by).IsRequired();
            entityTypeBuilder.Property(q => q.created_date).IsRequired().HasColumnType("timestamp without time zone");
            entityTypeBuilder.Property(q => q.modified_date).HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente).HasDatabaseName("IX_calculo_generacion_documento_id_expediente");
        }
    }
}
