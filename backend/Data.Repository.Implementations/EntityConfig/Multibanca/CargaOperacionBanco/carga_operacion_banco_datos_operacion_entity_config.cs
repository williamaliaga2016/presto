using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco
{
    internal class carga_operacion_banco_datos_operacion_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<carga_operacion_banco_datos_operacion_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("carga_operacion_banco_datos_operacion");

            entityTypeBuilder.HasKey(q => q.id_carga_operacion_banco_datos_operacion);

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco_datos_operacion)
                .HasColumnName("id_carga_operacion_banco_datos_operacion")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco)
                .HasColumnName("id_carga_operacion_banco")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.nro_mutuo)
                .HasColumnName("nro_mutuo");

            entityTypeBuilder.Property(q => q.tipo_operacion)
                .HasColumnName("tipo_operacion")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.nro_registro)
                .HasColumnName("nro_registro");

            entityTypeBuilder.Property(q => q.ult_clasif_al)
                .HasColumnName("ult_clasif_al")
                .HasMaxLength(200);

            entityTypeBuilder.Property(q => q.segmento)
                .HasColumnName("segmento")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.canal_venta)
                .HasColumnName("canal_venta")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.nro_op_cartera)
                .HasColumnName("nro_op_cartera");

            entityTypeBuilder.Property(q => q.modelo_operacion)
                .HasColumnName("modelo_operacion")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tipo_carpeta)
                .HasColumnName("tipo_carpeta")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.propietario)
                .HasColumnName("propietario")
                .HasMaxLength(150);

            entityTypeBuilder.Property(q => q.inmobiliaria)
                .HasColumnName("inmobiliaria")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.glosa_producto)
                .HasColumnName("glosa_producto")
                .HasMaxLength(50);

            entityTypeBuilder.Property(e => e.codigo_producto_comercial)
                .HasColumnName("codigo_producto_comercial")
                .HasMaxLength(50);

            entityTypeBuilder.Property(e => e.nro_piloto)
                .HasColumnName("nro_piloto");

            entityTypeBuilder.Property(e => e.banco_alzante)
                .HasColumnName("banco_alzante")
                .HasMaxLength(50);

            entityTypeBuilder.Property(e => e.nombre_proyecto)
                .HasColumnName("nombre_proyecto")
                .HasMaxLength(250);

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
                .HasDatabaseName("ix_cob_datos_operacion_id_carga_operacion_banco");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_cob_datos_operacion_id_expediente");

            entityTypeBuilder.Property(q => q.id_scoring)
                .HasColumnName("id_scoring")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.codigo_asesor)
                .HasColumnName("codigo_asesor")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.codigo_oficina)
                .HasColumnName("codigo_oficina")
                .HasMaxLength(20);

            entityTypeBuilder.Property(q => q.descripcion_oficina)
                .HasColumnName("descripcion_oficina")
                .HasMaxLength(200);

            entityTypeBuilder.Property(q => q.canal_originacion)
                .HasColumnName("canal_originacion")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tipo_inmueble)
                .HasColumnName("tipo_inmueble")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.estado_inmueble)
                .HasColumnName("estado_inmueble")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.descripcion_estado_inmueble)
                .HasColumnName("descripcion_estado_inmueble")
                .HasMaxLength(200);

            entityTypeBuilder.Property(q => q.codigo_proyecto)
                .HasColumnName("codigo_proyecto")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.descripcion_proyecto)
                .HasColumnName("descripcion_proyecto")
                .HasMaxLength(300);

            entityTypeBuilder.HasIndex(q => q.id_scoring)
                .HasDatabaseName("IX_cob_op_id_scoring");

            entityTypeBuilder.HasIndex(q => q.codigo_oficina)
                .HasDatabaseName("IX_cob_op_codigo_oficina");
        }
    }
}
