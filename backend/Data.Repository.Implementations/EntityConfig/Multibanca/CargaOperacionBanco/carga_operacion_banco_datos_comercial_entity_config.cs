using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco
{
    internal class carga_operacion_banco_datos_comercial_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<carga_operacion_banco_datos_comercial_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("carga_operacion_banco_datos_comercial");

            entityTypeBuilder.HasKey(q => q.id_carga_operacion_banco_datos_comercial);

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco_datos_comercial)
                .HasColumnName("id_carga_operacion_banco_datos_comercial")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco)
                .HasColumnName("id_carga_operacion_banco")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.codigo_ejecutivo)
                .HasColumnName("codigo_ejecutivo");

            entityTypeBuilder.Property(q => q.login_ejecutivo)
                .HasColumnName("login_ejecutivo")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.nombre_ejecutivo)
                .HasColumnName("nombre_ejecutivo")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.rut_ejecutivo)
                .HasColumnName("rut_ejecutivo");

            entityTypeBuilder.Property(q => q.codigo_oficina)
                .HasColumnName("codigo_oficina");

            entityTypeBuilder.Property(q => q.nombre_oficina)
                .HasColumnName("nombre_oficina")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.codigo_curse)
                .HasColumnName("codigo_curse");

            entityTypeBuilder.Property(q => q.glosa_curse)
                .HasColumnName("glosa_curse")
                .HasMaxLength(250);

            entityTypeBuilder.Property(q => q.codigo_ejecutivo_curse)
                .HasColumnName("codigo_ejecutivo_curse");

            entityTypeBuilder.Property(q => q.login_ejecutivo_curse)
                .HasColumnName("login_ejecutivo_curse")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.nombre_ejecutivo_curse)
                .HasColumnName("nombre_ejecutivo_curse")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.rut_ejecutivo_curse)
                .HasColumnName("rut_ejecutivo_curse");

            entityTypeBuilder.Property(q => q.rut_banco)
                .HasColumnName("rut_banco");

            entityTypeBuilder.Property(q => q.renovacion_urbana)
                .HasColumnName("renovacion_urbana")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.nombre_banco)
                .HasColumnName("nombre_banco")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.tipo_hipoteca)
                .HasColumnName("tipo_hipoteca")
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
                .HasDatabaseName("ix_cob_datos_comercial_id_carga_operacion_banco");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_cob_datos_comercial_id_expediente");

            entityTypeBuilder.Property(q => q.correo_declarativo_cliente)
                .HasColumnName("correo_declarativo_cliente")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.numero_telefono_declarativo)
                .HasColumnName("numero_telefono_declarativo")
                .HasMaxLength(20);
        }
    }
}
