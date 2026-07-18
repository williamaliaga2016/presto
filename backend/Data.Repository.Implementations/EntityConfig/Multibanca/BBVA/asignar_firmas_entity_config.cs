using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.BBVA;

internal static class asignar_firmas_entity_config
{
    public static void SetEntityBuilder(EntityTypeBuilder<asignar_firmas_peritos_abogados> b)
    {
        b.ToTable("asignar_firmas_peritos_abogados");
        b.HasKey(x => x.id);
        b.Property(x => x.id).ValueGeneratedOnAdd();
        b.Property(x => x.id_actividad).HasMaxLength(100).HasDefaultValue("ACT_ASIGNAR_FIRMAS");
        b.Property(x => x.tipo_cliente).HasMaxLength(100);
        b.Property(x => x.codigo_ejecutivo_solicitante).HasMaxLength(50);
        b.Property(x => x.oficina_solicitante).HasMaxLength(50);
        b.Property(x => x.tipo_solicitud_avaluo).HasMaxLength(50);
        b.Property(x => x.tipo_tramite_eett).HasMaxLength(50);
        b.Property(x => x.nombre_firma_supervisor).HasMaxLength(200);
        b.Property(x => x.telefono_firma).HasMaxLength(50);
        b.Property(x => x.email_firma).HasMaxLength(150);
        b.Property(x => x.valor_avaluo).HasColumnType("numeric(18,2)");
        b.Property(x => x.valor_total_consignar).HasColumnType("numeric(18,2)");
        b.Property(x => x.opciones_recaudo).HasMaxLength(200);
        b.Property(x => x.numero_recaudo).HasMaxLength(100);
        b.Property(x => x.banco).HasMaxLength(100);
        b.Property(x => x.nombre_abogado).HasMaxLength(200);
        b.Property(x => x.telefono_abogado).HasMaxLength(50);
        b.Property(x => x.valor_estudio_titulos).HasColumnType("numeric(18,2)");
        b.Property(x => x.tipo_cuenta_abogado).HasMaxLength(50);
        b.Property(x => x.numero_cuenta_abogado).HasMaxLength(100);
        b.Property(x => x.checklist_documentos_solicitar)
            .HasColumnName("checklist_documentos_solicitar")
            .HasColumnType("text");
        b.Property(x => x.is_active).HasDefaultValue(true);
        b.Property(x => x.row_status).HasDefaultValue(true);
        b.Property(x => x.created_date).HasColumnType("timestamp without time zone").HasDefaultValueSql("NOW()");
        b.Property(x => x.modified_date).HasColumnType("timestamp without time zone");
        b.HasIndex(x => x.id_expediente).HasDatabaseName("idx_afpa_expediente");
    }
}
