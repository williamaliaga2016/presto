using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class gestion_rectificatoria_escritura_firmada_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<gestion_rectificatoria_escritura_firmada_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("gestion_rectificatoria_escritura_firmada");

            entityTypeBuilder.HasKey(q => q.id_gestion_rectificatoria_escritura_firmada);

            entityTypeBuilder.Property(q => q.id_gestion_rectificatoria_escritura_firmada)
                .HasColumnName("id_gestion_rectificatoria_escritura_firmada")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_rectificatoria_firma_comprador_vendedor)
                .HasColumnName("id_rectificatoria_firma_comprador_vendedor")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.enviar_tipo_reparo)
                .HasColumnName("enviar_tipo_reparo")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.vb_solicitado_fiscalia)
                .HasColumnName("vb_solicitado_fiscalia")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_usuario_solicitante)
                .HasColumnName("id_usuario_solicitante")
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
            entityTypeBuilder.HasIndex(q => q.id_rectificatoria_firma_comprador_vendedor)
                .HasDatabaseName("ix_gestion_rectificatoria_escritura_firmada_rectificatoria_firma");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_gestion_rectificatoria_escritura_firmada_expediente");

            entityTypeBuilder.HasIndex(q => q.id_usuario_solicitante)
                .HasDatabaseName("ix_gestion_rectificatoria_escritura_firmada_usuario_solicitante");
        }
    }
}