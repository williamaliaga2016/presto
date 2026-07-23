using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class revisar_ep_abogado_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<revisar_ep_abogado_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("revisar_ep_abogado_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entityTypeBuilder.Property(q => q.id_expediente)
            .HasColumnName("id_expediente")
            .IsRequired();

        entityTypeBuilder.Property(q => q.id_actividad)
            .HasColumnName("id_actividad")
            .HasMaxLength(100);

        // Campo editable
        entityTypeBuilder.Property(q => q.representante_legal)
            .HasColumnName("representante_legal")
            .HasMaxLength(200);

        // Compuerta de Conformidad
        entityTypeBuilder.Property(q => q.ep_conforme)
            .HasColumnName("ep_conforme")
            .HasMaxLength(2);

        // Campos condicionales (obligatorios cuando ep_conforme = "NO")
        entityTypeBuilder.Property(q => q.tipologia)
            .HasColumnName("tipologia")
            .HasMaxLength(150);

        entityTypeBuilder.Property(q => q.casuistica)
            .HasColumnName("casuistica")
            .HasMaxLength(150);

        entityTypeBuilder.Property(q => q.observaciones_legales)
            .HasColumnName("observaciones_legales")
            .HasColumnType("text");

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
            .HasDatabaseName("idx_rea_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
