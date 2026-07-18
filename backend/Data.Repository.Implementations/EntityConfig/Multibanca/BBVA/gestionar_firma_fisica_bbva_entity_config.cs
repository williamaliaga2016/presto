using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class gestionar_firma_fisica_bbva_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<gestionar_firma_fisica_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("gestionar_firma_fisica");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.motorizado_asignado).HasColumnName("motorizado_asignado").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.fecha_gestoria).HasColumnName("fecha_gestoria").HasColumnType("date");
        entityTypeBuilder.Property(q => q.resultado_gestoria).HasColumnName("resultado_gestoria").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasColumnType("text");
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_gff_expediente");
    }
}
