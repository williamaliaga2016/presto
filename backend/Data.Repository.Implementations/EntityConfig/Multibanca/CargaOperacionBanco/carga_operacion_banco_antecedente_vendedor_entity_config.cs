using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco
{
    internal class carga_operacion_banco_antecedente_vendedor_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<carga_operacion_banco_antecedente_vendedor_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("carga_operacion_banco_antecedente_vendedor");

            entityTypeBuilder.HasKey(q => q.id_carga_operacion_banco_antecedente_vendedor);

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco_antecedente_vendedor)
                .HasColumnName("id_carga_operacion_banco_antecedente_vendedor")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco)
                .HasColumnName("id_carga_operacion_banco")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.rut)
                .HasColumnName("rut")
                .HasMaxLength(12);

            entityTypeBuilder.Property(q => q.tipo_vendedor)
                .HasColumnName("tipo_vendedor")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.razon_social)
                .HasColumnName("razon_social")
                .HasMaxLength(250);

            entityTypeBuilder.Property(q => q.nombres)
                .HasColumnName("nombres")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.apellido_paterno)
                .HasColumnName("apellido_paterno")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.apellido_materno)
                .HasColumnName("apellido_materno")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.fecha_nacimiento)
                .HasColumnName("fecha_nacimiento")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.Property(q => q.genero)
                .HasColumnName("genero")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.estado_civil)
                .HasColumnName("estado_civil")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.relacion_titular)
                .HasColumnName("relacion_titular")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.direccion)
                .HasColumnName("direccion")
                .HasMaxLength(250);

            entityTypeBuilder.Property(q => q.region)
                .HasColumnName("region")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.comuna)
                .HasColumnName("comuna")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            entityTypeBuilder.Property(q => q.email)
                .HasColumnName("email")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.nacionalidad)
                .HasColumnName("nacionalidad")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.profesion)
                .HasColumnName("profesion")
                .HasMaxLength(150);

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

            entityTypeBuilder.HasIndex(q => q.id_carga_operacion_banco)
                .HasDatabaseName("ix_cob_antecedente_vendedor_id_carga_operacion_banco");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_cob_antecedente_vendedor_id_expediente");

            entityTypeBuilder.HasIndex(q => q.rut)
                .HasDatabaseName("ix_cob_antecedente_vendedor_rut");
        }
    }
}
