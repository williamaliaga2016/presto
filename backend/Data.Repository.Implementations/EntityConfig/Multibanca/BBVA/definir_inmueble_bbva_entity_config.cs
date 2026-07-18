using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class definir_inmueble_bbva_entity_config
{
    public static void SetEntityBuilder(
        EntityTypeBuilder<definir_inmueble_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("definir_inmueble_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.cliente_cuenta_inmueble_definido)
            .HasColumnName("cliente_cuenta_inmueble_definido");
        entityTypeBuilder.Property(q => q.constructora).HasColumnName("constructora").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.fecha_estimada_entrega)
            .HasColumnName("fecha_estimada_entrega")
            .HasColumnType("date");
        entityTypeBuilder.Property(q => q.estatus_general).HasColumnName("estatus_general").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.motivo_devolucion).HasColumnName("motivo_devolucion").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date)
            .HasColumnName("created_date")
            .HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("IX_definir_inmueble_expediente");
    }
}
