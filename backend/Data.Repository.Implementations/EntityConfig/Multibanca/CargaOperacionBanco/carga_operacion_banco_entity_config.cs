using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco
{
    internal class carga_operacion_banco_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<carga_operacion_banco_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("carga_operacion_banco");

            entityTypeBuilder.HasKey(q => q.id_carga_operacion_banco);

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco)
                .HasColumnName("id_carga_operacion_banco")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
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
                .HasDatabaseName("ix_carga_operacion_banco_id_expediente");
        }
    }
}
