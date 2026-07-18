using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    internal static class interviniente_bbva_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<interviniente_bbva_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("interviniente_bbva", "public");

            // Llave Primaria
            entityTypeBuilder.HasKey(q => q.id);

            entityTypeBuilder.Property(q => q.id)
                .HasColumnName("id")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_actividad)
                .HasColumnName("id_actividad")
                .HasMaxLength(50)
                .HasDefaultValue("ACT_DEVOLUCION_VB_COMERCIAL")
                .IsRequired();

            entityTypeBuilder.Property(q => q.nombre_completo)
                .HasColumnName("nombre_completo")
                .HasMaxLength(250)
                .IsRequired();

            entityTypeBuilder.Property(q => q.tipo_identificacion)
                .HasColumnName("tipo_identificacion")
                .HasMaxLength(20)
                .IsRequired();

            entityTypeBuilder.Property(q => q.numero_identificacion)
                .HasColumnName("numero_identificacion")
                .HasMaxLength(30)
                .IsRequired();

            entityTypeBuilder.Property(q => q.telefono)
                .HasColumnName("telefono")
                .HasMaxLength(30);

            entityTypeBuilder.Property(q => q.correo_electronico)
                .HasColumnName("correo_electronico")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.is_active)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            entityTypeBuilder.Property(q => q.row_status)
                .HasColumnName("row_status")
                .HasDefaultValue(true)
                .IsRequired();

            entityTypeBuilder.Property(q => q.created_by)
                .HasColumnName("created_by")
                .IsRequired();

            entityTypeBuilder.Property(q => q.created_date)
                .HasColumnName("created_date")
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            entityTypeBuilder.Property(q => q.modified_by)
                .HasColumnName("modified_by");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnName("modified_date")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("idx_interviniente_expediente");
        }
    }
}
