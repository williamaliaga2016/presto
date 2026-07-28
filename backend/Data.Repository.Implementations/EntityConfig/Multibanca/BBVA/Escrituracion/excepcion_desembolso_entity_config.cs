using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class excepcion_desembolso_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<excepcion_desembolso_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("excepcion_desembolso");
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

        entityTypeBuilder.Property(q => q.excepcion_autorizada)
            .HasColumnName("excepcion_autorizada")
            .HasMaxLength(2);

        entityTypeBuilder.Property(q => q.requiere_vobo_gerencia)
            .HasColumnName("requiere_vobo_gerencia")
            .HasMaxLength(2);

        entityTypeBuilder.Property(q => q.confirmacion_excepcion)
            .HasColumnName("confirmacion_excepcion")
            .HasDefaultValue(false);

        entityTypeBuilder.Property(q => q.observaciones_excepcion)
            .HasColumnName("observaciones_excepcion")
            .HasMaxLength(1000);

        entityTypeBuilder.Property(q => q.origen_caso)
            .HasColumnName("origen_caso")
            .HasMaxLength(50);

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

        // Índice UNIQUE parcial: un solo registro activo por expediente
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_excepcion_desembolso_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
