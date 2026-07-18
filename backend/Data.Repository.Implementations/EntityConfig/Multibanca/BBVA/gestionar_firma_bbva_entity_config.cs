using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class gestionar_firma_bbva_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<gestionar_firma_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("gestionar_firma");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.requiere_firma_electronica).HasColumnName("requiere_firma_electronica");
        entityTypeBuilder.Property(q => q.firma_electronica_realizada).HasColumnName("firma_electronica_realizada");
        entityTypeBuilder.Property(q => q.nombre_cliente_firma).HasColumnName("nombre_cliente_firma").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.nombre_solicitante_firma).HasColumnName("nombre_solicitante_firma").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.franja_horaria).HasColumnName("franja_horaria");
        entityTypeBuilder.Property(q => q.direccion_firma).HasColumnName("direccion_firma").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.descripcion_tramite).HasColumnName("descripcion_tramite").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.fecha_programacion).HasColumnName("fecha_programacion").HasColumnType("date");
        entityTypeBuilder.Property(q => q.ciudad_cliente).HasColumnName("ciudad_cliente").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.tipo_credito_firma).HasColumnName("tipo_credito_firma").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasColumnType("text");
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_gf_expediente");
    }
}