using Data.Repository.Interfaces.Entities.Multibanca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca
{
    internal class generar_finiquito_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<generar_finiquito_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("generar_finiquito");

            entityTypeBuilder.HasKey(q => q.id_generar_finiquito);

            entityTypeBuilder.Property(q => q.id_generar_finiquito)
                .HasColumnName("id_generar_finiquito")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            // Propiedad
            entityTypeBuilder.Property(q => q.fojas_propiedad)
                .HasColumnName("fojas_propiedad")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.numero_propiedad)
                .HasColumnName("numero_propiedad")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.año_propiedad)
                .HasColumnName("año_propiedad")
                .HasMaxLength(10);

            // Hipoteca
            entityTypeBuilder.Property(q => q.fojas_hipoteca)
                .HasColumnName("fojas_hipoteca")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.numero_hipoteca)
                .HasColumnName("numero_hipoteca")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.año_hipoteca)
                .HasColumnName("año_hipoteca")
                .HasMaxLength(10);

            // Prohibicion
            entityTypeBuilder.Property(q => q.fojas_prohibicion)
                .HasColumnName("fojas_prohibicion")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.numero_prohibicion)
                .HasColumnName("numero_prohibicion")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.año_prohibicion)
                .HasColumnName("año_prohibicion")
                .HasMaxLength(10);

            // Hipoteca 2do Grado
            entityTypeBuilder.Property(q => q.fojas_hipoteca_2grado)
                .HasColumnName("fojas_hipoteca_2grado")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.numero_hipoteca_2grado)
                .HasColumnName("numero_hipoteca_2grado")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.año_hipoteca_2grado)
                .HasColumnName("año_hipoteca_2grado")
                .HasMaxLength(10);

            entityTypeBuilder.Property(q => q.observaciones)
                .HasColumnName("observaciones")
                .HasMaxLength(1000);

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
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            entityTypeBuilder.Property(q => q.modified_by)
                .HasColumnName("modified_by");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnName("modified_date")
                .HasColumnType("timestamp without time zone");

            // Índices
            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_generar_finiquito_expediente");
        }
    }
}