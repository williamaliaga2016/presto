using Data.Repository.Interfaces.Entities.Multibanca.BBVA.ValidarIntegracionDocumentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.ValidarIntegracionDocumentos
{
    internal static class validar_integracion_documentos_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<validar_integracion_documentos_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("validar_integracion_documentos", "public");

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
                .HasMaxLength(100)
                .IsRequired();

            entityTypeBuilder.Property(q => q.documentos_correctos)
                .HasColumnName("documentos_correctos");

            entityTypeBuilder.Property(q => q.credito_condicionado)
                .HasColumnName("credito_condicionado")
                .HasDefaultValue(false)
                .IsRequired();

            entityTypeBuilder.Property(q => q.motivo_devolucion)
                .HasColumnName("motivo_devolucion")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.observaciones)
                .HasColumnName("observaciones");

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
                .IsRequired();

            entityTypeBuilder.Property(q => q.modified_by)
                .HasColumnName("modified_by");

            entityTypeBuilder.Property(q => q.modified_date)
                .HasColumnName("modified_date")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("idx_vid_expediente");
        }
    }
}
