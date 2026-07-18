using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_antecedente_credito_entity
    {
        [Key]
        public int id_carga_operacion_banco_antecedente_credito { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public string? tipo_prestamo { get; set; }

        public decimal? factor_conversion_uf { get; set; }

        public string? destino_credito { get; set; }

        public decimal? monto_solicitado { get; set; }

        public string? tipo_tasa { get; set; }

        public decimal? tasa { get; set; }

        public int? plazo { get; set; }

        public DateTime? fecha_inicio { get; set; }

        public decimal? monto_nominal { get; set; }

        public decimal? monto_residual { get; set; }

        public int? plazo_primer_periodo { get; set; }

        public int? periodo_gracia { get; set; }

        public decimal? comision { get; set; }

        public int? plazo_segundo_periodo { get; set; }

        public decimal? tasa_primer_periodo { get; set; }

        public int? meses_sabaticos { get; set; }

        public string? variabilidad_tasa { get; set; }

        public decimal? tasa_segundo_periodo { get; set; }

        public string? moneda { get; set; }

        public string? tipo_tasa_mixta_prod_com { get; set; }

        public decimal? tasa_maxima { get; set; }

        public string? codigo_producto_cartera { get; set; }

        public string? indicador_segunda_vivienda { get; set; }

        public string? tipo_financiamiento { get; set; }

        public decimal? precio_venta_pesos { get; set; }

        public string? precio_venta_moneda_original { get; set; }

        public int? cantidad_meses_sin_vencimiento { get; set; }

        public int? indicador_cred_comp { get; set; }

        public string? indicador_pac { get; set; }

        public string? tipo_tasa_aplic_contab { get; set; }

        public long? numero_cuenta_gastos { get; set; }

        public decimal? prestamo_maximo { get; set; }

        // BBVA COLOMBIA
        public string? id_tipo_sub_producto { get; set; }
        public decimal? monto_otorgado { get; set; }
        public DateTime? fecha_aprobacion { get; set; }
        public string? condiciones_organismo_decisor { get; set; }
        public bool? aplica_fast_track { get; set; }
        public bool? aplica_leasing { get; set; }
        public bool? aplica_cobertura { get; set; }
        public bool? aplica_compra_cartera { get; set; }
        public bool? aplica_remodelacion { get; set; }
        public bool? gente_bbva { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
