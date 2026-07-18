using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class registro_contacto_bbva_entity_config
{
    public static void SetEntityBuilder(
        EntityTypeBuilder<registro_contacto_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("registro_contacto_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id).HasColumnName("id").ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.id_usuario).HasColumnName("id_usuario");
        entityTypeBuilder.Property(q => q.canal_contacto).HasColumnName("canal_contacto").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.resultado_contacto).HasColumnName("resultado_contacto").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.nro_contacto).HasColumnName("nro_contacto");
        entityTypeBuilder.Property(q => q.detalle_contacto).HasColumnName("detalle_contacto").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.inmueble_definido).HasColumnName("inmueble_definido");
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones");
        entityTypeBuilder.Property(q => q.fecha_contacto).HasColumnName("fecha_contacto").HasColumnType("date");
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(new[] { "id_expediente", "id_actividad" })
            .HasDatabaseName("IX_registro_contacto_expediente");
    }
}
