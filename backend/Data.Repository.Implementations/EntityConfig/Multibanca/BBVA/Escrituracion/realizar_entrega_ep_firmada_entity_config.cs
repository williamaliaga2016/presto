using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class realizar_entrega_ep_firmada_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<realizar_entrega_ep_firmada_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("realizar_entrega_ep_firmada");
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

        entityTypeBuilder.Property(q => q.entregado_a)
            .HasColumnName("entregado_a")
            .HasMaxLength(200);

        entityTypeBuilder.Property(q => q.aplica_excepcion)
            .HasColumnName("aplica_excepcion")
            .HasMaxLength(2);

        entityTypeBuilder.Property(q => q.observaciones)
            .HasColumnName("observaciones")
            .HasMaxLength(500);

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

        // Índice UNIQUE parcial
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_realizar_entrega_ep_firmada_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
