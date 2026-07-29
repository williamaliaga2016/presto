using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class vobo_gerencia_coh_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<vobo_gerencia_coh_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("vobo_gerencia_coh_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entityTypeBuilder.Property(q => q.id_expediente)
            .HasColumnName("id_expediente")
            .IsRequired();

        entityTypeBuilder.Property(q => q.id_actividad)
            .HasColumnName("id_actividad")
            .HasMaxLength(100)
            .IsRequired();

        entityTypeBuilder.Property(q => q.concepto_vobo)
            .HasColumnName("concepto_vobo")
            .HasMaxLength(20);

        entityTypeBuilder.Property(q => q.observaciones)
            .HasColumnName("observaciones")
            .HasMaxLength(1000);

        // Auditoría
        entityTypeBuilder.Property(q => q.is_active)
            .HasColumnName("is_active")
            .IsRequired();

        entityTypeBuilder.Property(q => q.row_status)
            .HasColumnName("row_status")
            .IsRequired();

        entityTypeBuilder.Property(q => q.created_by)
            .HasColumnName("created_by")
            .IsRequired();

        entityTypeBuilder.Property(q => q.created_date)
            .HasColumnName("created_date")
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        entityTypeBuilder.Property(q => q.modified_by)
            .HasColumnName("modified_by");

        entityTypeBuilder.Property(q => q.modified_date)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp without time zone");

        // Índice parcial único: un solo registro activo por expediente
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_vobo_gerencia_coh_bbva_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
