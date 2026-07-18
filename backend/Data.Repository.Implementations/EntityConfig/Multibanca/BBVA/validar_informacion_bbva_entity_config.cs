using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal class validar_informacion_bbva_entity_config
{
    public static void SetEntityBuilder(
        EntityTypeBuilder<validar_informacion_bbva> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("validar_informacion_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        entityTypeBuilder.Property(q => q.id_expediente).HasColumnName("id_expediente");
        entityTypeBuilder.Property(q => q.id_actividad).HasColumnName("id_actividad").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.tipo_id_t1).HasColumnName("tipo_id_t1").HasMaxLength(10);
        entityTypeBuilder.Property(q => q.numero_id_t1).HasColumnName("numero_id_t1").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.nombre_completo_t1).HasColumnName("nombre_completo_t1").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.celular_t1).HasColumnName("celular_t1").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.telefono_t1).HasColumnName("telefono_t1").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.email_t1).HasColumnName("email_t1").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.direccion_t1).HasColumnName("direccion_t1").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.departamento_t1).HasColumnName("departamento_t1").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.municipio_t1).HasColumnName("municipio_t1").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.situacion_laboral_t1).HasColumnName("situacion_laboral_t1").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.cliente_nomina_t1).HasColumnName("cliente_nomina_t1");
        entityTypeBuilder.Property(q => q.tipo_id_t2).HasColumnName("tipo_id_t2").HasMaxLength(10);
        entityTypeBuilder.Property(q => q.numero_id_t2).HasColumnName("numero_id_t2").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.nombre_completo_t2).HasColumnName("nombre_completo_t2").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.celular_t2).HasColumnName("celular_t2").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.email_t2).HasColumnName("email_t2").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.tipo_id_t3).HasColumnName("tipo_id_t3").HasMaxLength(10);
        entityTypeBuilder.Property(q => q.numero_id_t3).HasColumnName("numero_id_t3").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.nombre_completo_t3).HasColumnName("nombre_completo_t3").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.celular_t3).HasColumnName("celular_t3").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.email_t3).HasColumnName("email_t3").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.inmueble_definido).HasColumnName("inmueble_definido");
        entityTypeBuilder.Property(q => q.tipo_inmueble).HasColumnName("tipo_inmueble").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.estado_inmueble).HasColumnName("estado_inmueble").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.constructora).HasColumnName("constructora").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.es_constructora_vip).HasColumnName("es_constructora_vip");
        entityTypeBuilder.Property(q => q.codigo_proyecto).HasColumnName("codigo_proyecto").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.descripcion_proyecto).HasColumnName("descripcion_proyecto").HasMaxLength(300);
        entityTypeBuilder.Property(q => q.departamento_inmueble).HasColumnName("departamento_inmueble").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.municipio_inmueble).HasColumnName("municipio_inmueble").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.fecha_estimada_entrega).HasColumnName("fecha_estimada_entrega").HasColumnType("date");
        entityTypeBuilder.Property(q => q.estatus_general).HasColumnName("estatus_general").HasMaxLength(100);
        entityTypeBuilder.Property(q => q.origen_devolucion).HasColumnName("origen_devolucion").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.requiere_definir_inmueble).HasColumnName("requiere_definir_inmueble");
        entityTypeBuilder.Property(q => q.requiere_carga_cliente).HasColumnName("requiere_carga_cliente");
        entityTypeBuilder.Property(q => q.requiere_carga_constructora).HasColumnName("requiere_carga_constructora");
        entityTypeBuilder.Property(q => q.tipo_credito).HasColumnName("tipo_credito").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.tiene_garantia).HasColumnName("tiene_garantia");
        entityTypeBuilder.Property(q => q.garantia_constituida).HasColumnName("garantia_constituida");
        entityTypeBuilder.Property(q => q.monto_otorgado_vi).HasColumnName("monto_otorgado_vi").HasColumnType("numeric(18,2)");
        entityTypeBuilder.Property(q => q.monto_otorgado_vivienda_original).HasColumnName("monto_otorgado_vivienda_original").HasColumnType("numeric(18,2)");
        entityTypeBuilder.Property(q => q.correo_declarativo).HasColumnName("correo_declarativo").HasMaxLength(150);
        entityTypeBuilder.Property(q => q.telefono_declarativo).HasColumnName("telefono_declarativo").HasMaxLength(20);
        entityTypeBuilder.Property(q => q.codigo_oficina).HasColumnName("codigo_oficina").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.descripcion_oficina).HasColumnName("descripcion_oficina").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.codigo_asesor).HasColumnName("codigo_asesor").HasMaxLength(50);
        entityTypeBuilder.Property(q => q.motivo_devolucion).HasColumnName("motivo_devolucion").HasMaxLength(200);
        entityTypeBuilder.Property(q => q.observaciones).HasColumnName("observaciones").HasMaxLength(500);
        entityTypeBuilder.Property(q => q.is_active).HasColumnName("is_active");
        entityTypeBuilder.Property(q => q.row_status).HasColumnName("row_status");
        entityTypeBuilder.Property(q => q.created_by).HasColumnName("created_by");
        entityTypeBuilder.Property(q => q.created_date).HasColumnName("created_date").HasColumnType("timestamp without time zone");
        entityTypeBuilder.Property(q => q.modified_by).HasColumnName("modified_by");
        entityTypeBuilder.Property(q => q.modified_date).HasColumnName("modified_date").HasColumnType("timestamp without time zone");

        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("IX_validar_info_expediente");
    }
}
