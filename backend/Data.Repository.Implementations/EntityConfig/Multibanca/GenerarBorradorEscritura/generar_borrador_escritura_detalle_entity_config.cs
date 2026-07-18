using Data.Repository.Interfaces.Entities.Multibanca.GenerarBorradorEscritura;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.GenerarBorradorEscritura
{
    internal class generar_borrador_escritura_detalle_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<generar_borrador_escritura_detalle_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("generar_borrador_escritura_detalle");

            entityTypeBuilder.HasKey(q => q.id_generar_borrador_escritura_detalle_entity);

            entityTypeBuilder.Property(q => q.id_generar_borrador_escritura_detalle_entity)
                .HasColumnName("id_generar_borrador_escritura_detalle_entity")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_generar_borrador_escritura)
                .HasColumnName("id_generar_borrador_escritura")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_datos_operacion_fiador_garante)
                .HasColumnName("id_datos_operacion_fiador_garante")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_rol_comparecencia)
                .HasColumnName("id_rol_comparecencia")
                .IsRequired();

            entityTypeBuilder.Property(q => q.requiere_firma)
                .HasColumnName("requiere_firma")
                .IsRequired();

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
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            entityTypeBuilder.Property(q => q.modified_by)
                .HasColumnName("modified_by");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnName("modified_date")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_gbe_detalle_id_expediente");

            entityTypeBuilder.HasIndex(q => q.id_generar_borrador_escritura)
                .HasDatabaseName("ix_gbe_detalle_id_gbe");

            entityTypeBuilder.HasIndex(q => q.id_datos_operacion_fiador_garante)
                .HasDatabaseName("ix_gbe_detalle_id_fiador_garante");
        }
    }
}