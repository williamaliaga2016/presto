using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class carta_aprobacion_bbva_entity_config
{
    public static void SetEntityBuilder(
        EntityTypeBuilder<carta_aprobacion_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("carta_aprobacion_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id).HasColumnName("id").ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_tipo_sub_producto).HasColumnName("id_tipo_sub_producto").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.modelo_carta).HasColumnName("modelo_carta");
        entityTypeBuilder.Property(q => q.nombre_archivo_docx).HasColumnName("nombre_archivo_docx").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.nombre_archivo_pdf).HasColumnName("nombre_archivo_pdf").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.url_docx).HasColumnName("url_docx").HasMaxLength(1000);
        entityTypeBuilder.Property(q => q.url_pdf).HasColumnName("url_pdf").HasMaxLength(1000);
        entityTypeBuilder.Property(q => q.estado).HasColumnName("estado").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.error_detalle).HasColumnName("error_detalle");
        entityTypeBuilder.Property(q => q.version).HasColumnName("version");
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(new[] { "id_expediente", "estado" })
            .HasDatabaseName("IX_carta_aprobacion_expediente_estado");
    }
}
