using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class gestion_rectificatoria_solucion_reparo_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<gestion_rectificatoria_solucion_reparo_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("gestion_rectificatoria_solucion_reparo");

            entityTypeBuilder.HasKey(q => q.id_gestion_rectificatoria_solucion_reparo);

            entityTypeBuilder.Property(q => q.id_gestion_rectificatoria_solucion_reparo)
                .HasColumnName("id_gestion_rectificatoria_solucion_reparo")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_gestion_rectificatoria)
                .HasColumnName("id_gestion_rectificatoria")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_usuario_solicitante)
                .HasColumnName("id_usuario_solicitante")
                .IsRequired();

            // Nuevos campos
            entityTypeBuilder.Property(q => q.modificar_datos_memo)
                .HasColumnName("modificar_datos_memo")
                .IsRequired();

            entityTypeBuilder.Property(q => q.descontabilizar_operacion)
                .HasColumnName("descontabilizar_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.subsanar)
                .HasColumnName("subsanar")
                .IsRequired();

            entityTypeBuilder.Property(q => q.observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(1000);

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

            // Índices
            entityTypeBuilder.HasIndex(q => q.id_gestion_rectificatoria)
                .HasDatabaseName("ix_gestion_rectificatoria_solucion_reparo_gestion_rectificatoria");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_gestion_rectificatoria_solucion_reparo_expediente");

            entityTypeBuilder.HasIndex(q => q.id_usuario_solicitante)
                .HasDatabaseName("ix_gestion_rectificatoria_solucion_reparo_usuario_solicitante");
        }
    }
}