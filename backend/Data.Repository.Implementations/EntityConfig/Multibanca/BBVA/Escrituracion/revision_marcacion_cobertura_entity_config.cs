using Data.Repository.Interfaces.Entities.Multibanca.BBVA.Escrituracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA.Escrituracion;

internal class revision_marcacion_cobertura_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<revision_marcacion_cobertura_entity> entityTypeBuilder)
    {
        entityTypeBuilder.ToTable("revision_marcacion_cobertura_bbva");
        entityTypeBuilder.HasKey(q => q.id);
        entityTypeBuilder.Property(q => q.id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entityTypeBuilder.Property(q => q.id_expediente)
            .HasColumnName("id_expediente")
            .IsRequired();

        entityTypeBuilder.Property(q => q.id_actividad)
            .HasColumnName("id_actividad")
            .HasMaxLength(100)
            .IsRequired();

        entityTypeBuilder.Property(q => q.email_area_colocaciones)
            .HasColumnName("email_area_colocaciones")
            .HasMaxLength(150);

        entityTypeBuilder.Property(q => q.consecutivo)
            .HasColumnName("consecutivo")
            .HasMaxLength(50);

        entityTypeBuilder.Property(q => q.tipo_documento)
            .HasColumnName("tipo_documento")
            .HasMaxLength(10);

        entityTypeBuilder.Property(q => q.numero_documento)
            .HasColumnName("numero_documento")
            .HasMaxLength(30);

        entityTypeBuilder.Property(q => q.tipo_tramite)
            .HasColumnName("tipo_tramite")
            .HasMaxLength(30);

        entityTypeBuilder.Property(q => q.nombre)
            .HasColumnName("nombre")
            .HasMaxLength(200);

        entityTypeBuilder.Property(q => q.constructora)
            .HasColumnName("constructora")
            .HasMaxLength(200);

        entityTypeBuilder.Property(q => q.proyecto)
            .HasColumnName("proyecto")
            .HasMaxLength(200);

        entityTypeBuilder.Property(q => q.fecha_aceptacion_plataforma)
            .HasColumnName("fecha_aceptacion_plataforma")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.tipo_vivienda)
            .HasColumnName("tipo_vivienda")
            .HasMaxLength(50);

        entityTypeBuilder.Property(q => q.valor_subsidio)
            .HasColumnName("valor_subsidio")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.numero_obligacion)
            .HasColumnName("numero_obligacion")
            .HasMaxLength(50);

        entityTypeBuilder.Property(q => q.fecha_desembolso)
            .HasColumnName("fecha_desembolso")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.fecha_proximo_canon)
            .HasColumnName("fecha_proximo_canon")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.valor_desembolso)
            .HasColumnName("valor_desembolso")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.intereses_corrientes)
            .HasColumnName("intereses_corrientes")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.capital)
            .HasColumnName("capital")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.seguros)
            .HasColumnName("seguros")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.cuota_mensual)
            .HasColumnName("cuota_mensual")
            .HasColumnType("numeric(18,2)");

        entityTypeBuilder.Property(q => q.plazo)
            .HasColumnName("plazo");

        entityTypeBuilder.Property(q => q.observacion)
            .HasColumnName("observacion")
            .HasMaxLength(1000);

        entityTypeBuilder.Property(q => q.fecha_solicitud_marcacion)
            .HasColumnName("fecha_solicitud_marcacion")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.hora_solicitud_marcacion)
            .HasColumnName("hora_solicitud_marcacion")
            .HasMaxLength(5);

        entityTypeBuilder.Property(q => q.fecha_respuesta_marcacion)
            .HasColumnName("fecha_respuesta_marcacion")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.hora_respuesta_marcacion)
            .HasColumnName("hora_respuesta_marcacion")
            .HasMaxLength(5);

        entityTypeBuilder.Property(q => q.responsable_m5)
            .HasColumnName("responsable_m5")
            .HasMaxLength(150);

        entityTypeBuilder.Property(q => q.no_resolucion)
            .HasColumnName("no_resolucion")
            .HasMaxLength(50);

        entityTypeBuilder.Property(q => q.fecha_resolucion)
            .HasColumnName("fecha_resolucion")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.fecha_envio_resolucion)
            .HasColumnName("fecha_envio_resolucion")
            .HasColumnType("date");

        entityTypeBuilder.Property(q => q.estado_proceso)
            .HasColumnName("estado_proceso")
            .HasMaxLength(50);

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
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        entityTypeBuilder.Property(q => q.modified_by)
            .HasColumnName("modified_by");

        entityTypeBuilder.Property(q => q.modified_date)
            .HasColumnName("modified_date")
            .HasColumnType("timestamp without time zone");

        // Índice parcial único: un solo registro activo por expediente
        entityTypeBuilder.HasIndex(q => q.id_expediente)
            .HasDatabaseName("idx_revision_marcacion_cobertura_bbva_expediente")
            .IsUnique()
            .HasFilter("is_active = true AND row_status = true");
    }
}
