using Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Repository.Implementations.EntityConfig.Multibanca.CargaOperacionBanco
{
    internal class carga_operacion_banco_antecedente_credito_entity_config
    {
        public static void SetEntityBuilder(EntityTypeBuilder<carga_operacion_banco_antecedente_credito_entity> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("carga_operacion_banco_antecedente_credito");

            entityTypeBuilder.HasKey(q => q.id_carga_operacion_banco_antecedente_credito);

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco_antecedente_credito)
                .HasColumnName("id_carga_operacion_banco_antecedente_credito")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_carga_operacion_banco)
                .HasColumnName("id_carga_operacion_banco")
                .IsRequired();

            entityTypeBuilder.Property(q => q.id_expediente)
                .HasColumnName("id_expediente")
                .IsRequired();

            entityTypeBuilder.Property(q => q.tipo_prestamo)
                .HasColumnName("tipo_prestamo")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.factor_conversion_uf)
                .HasColumnName("factor_conversion_uf")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.destino_credito)
                .HasColumnName("destino_credito")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.monto_solicitado)
                .HasColumnName("monto_solicitado")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.tipo_tasa)
                .HasColumnName("tipo_tasa")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tasa)
                .HasColumnName("tasa")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.plazo)
                .HasColumnName("plazo");

            entityTypeBuilder.Property(q => q.fecha_inicio)
                .HasColumnName("fecha_inicio")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.Property(q => q.monto_nominal)
                .HasColumnName("monto_nominal")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.monto_residual)
                .HasColumnName("monto_residual")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.plazo_primer_periodo)
                .HasColumnName("plazo_primer_periodo");

            entityTypeBuilder.Property(q => q.periodo_gracia)
                .HasColumnName("periodo_gracia");

            entityTypeBuilder.Property(q => q.comision)
                .HasColumnName("comision")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.plazo_segundo_periodo)
                .HasColumnName("plazo_segundo_periodo");

            entityTypeBuilder.Property(q => q.tasa_primer_periodo)
                .HasColumnName("tasa_primer_periodo")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.meses_sabaticos)
                .HasColumnName("meses_sabaticos");

            entityTypeBuilder.Property(q => q.variabilidad_tasa)
                .HasColumnName("variabilidad_tasa")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.tasa_segundo_periodo)
                .HasColumnName("tasa_segundo_periodo")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.moneda)
                .HasColumnName("moneda")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tipo_tasa_mixta_prod_com)
                .HasColumnName("tipo_tasa_mixta_prod_com")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.tasa_maxima)
                .HasColumnName("tasa_maxima")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.codigo_producto_cartera)
                .HasColumnName("codigo_producto_cartera")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.indicador_segunda_vivienda)
                .HasColumnName("indicador_segunda_vivienda")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tipo_financiamiento)
                .HasColumnName("tipo_financiamiento")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.precio_venta_pesos)
                .HasColumnName("precio_venta_pesos")
                .HasColumnType("numeric(18, 6)");

            entityTypeBuilder.Property(q => q.precio_venta_moneda_original)
                .HasColumnName("precio_venta_moneda_original")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.cantidad_meses_sin_vencimiento)
                .HasColumnName("cantidad_meses_sin_vencimiento");

            entityTypeBuilder.Property(q => q.indicador_cred_comp)
                .HasColumnName("indicador_cred_comp");

            entityTypeBuilder.Property(q => q.indicador_pac)
                .HasColumnName("indicador_pac")
                .HasMaxLength(50);

            entityTypeBuilder.Property(q => q.tipo_tasa_aplic_contab)
                .HasColumnName("tipo_tasa_aplic_contab")
                .HasMaxLength(100);

            entityTypeBuilder.Property(q => q.numero_cuenta_gastos)
                .HasColumnName("numero_cuenta_gastos");

            entityTypeBuilder.Property(q => q.prestamo_maximo)
                .HasColumnName("prestamo_maximo")
                .HasColumnType("numeric(18, 6)");

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
                .HasDatabaseName("ix_cob_ant_credito_id_carga_operacion_banco");

            entityTypeBuilder.HasIndex(q => q.id_expediente)
                .HasDatabaseName("ix_cob_ant_credito_id_expediente");

            entityTypeBuilder.Property(q => q.id_tipo_sub_producto)
                .HasColumnName("id_tipo_sub_producto")
                .HasMaxLength(20);

            entityTypeBuilder.Property(q => q.monto_otorgado)
                .HasColumnName("monto_otorgado")
                .HasColumnType("numeric(18,2)");

            entityTypeBuilder.Property(q => q.fecha_aprobacion)
                .HasColumnName("fecha_aprobacion")
                .HasColumnType("timestamp without time zone");

            entityTypeBuilder.Property(q => q.condiciones_organismo_decisor)
                .HasColumnName("condiciones_organismo_decisor")
                .HasMaxLength(500);

            entityTypeBuilder.Property(q => q.aplica_fast_track)
                .HasColumnName("aplica_fast_track");

            entityTypeBuilder.Property(q => q.aplica_leasing)
                .HasColumnName("aplica_leasing");

            entityTypeBuilder.Property(q => q.aplica_cobertura)
                .HasColumnName("aplica_cobertura");

            entityTypeBuilder.Property(q => q.aplica_compra_cartera)
                .HasColumnName("aplica_compra_cartera");

            entityTypeBuilder.Property(q => q.aplica_remodelacion)
                .HasColumnName("aplica_remodelacion");

            entityTypeBuilder.Property(q => q.gente_bbva)
                .HasColumnName("gente_bbva");

            entityTypeBuilder.HasIndex(q => q.id_tipo_sub_producto)
                .HasDatabaseName("IX_cob_cred_subproducto");
        }
    }
}
