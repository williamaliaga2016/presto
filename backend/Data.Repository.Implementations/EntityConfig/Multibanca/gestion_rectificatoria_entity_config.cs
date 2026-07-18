using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class gestion_rectificatoria_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<gestion_rectificatoria_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("gestion_rectificatoria");

            entityTypeBuilder.HasKey(q => q.id_gestion_rectificatoria);

            entityTypeBuilder.Property(q => q.id_gestion_rectificatoria)
                .HasColumnName("id_gestion_rectificatoria")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.enviar_tipo_reparo)
                .HasColumnName("enviar_tipo_reparo")
                .HasMaxLength(100)
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

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_gestion_rectificatoria_expediente");
        }
    }
}