using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion
{
    public class revisar_datos_operacion_credito : base_auditoria
    {
        public int id_revisar_datos_operacion_credito { get; set; }

        public int id_revisar_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public bool? santiago { get; set; }

        public bool? regiones { get; set; }

        public string? tipo_operacion { get; set; }

        public bool? fines_generales { get; set; }

        public string? nombre_proyecto { get; set; }

        public bool? credito_segunda_vivienda { get; set; }

        public string? inmobiliaria { get; set; }

        public bool? vivienda_social { get; set; }

        public bool? dfl2 { get; set; }

        public bool? propietario_0_1_dfl2 { get; set; }

        public bool? recepcion_final_mayor_2 { get; set; }

        public decimal? porcentaje_impuesto { get; set; }

        public decimal? monto_credito_afecto { get; set; }

        public decimal? impuesto_a_pagar { get; set; }

        public string? observaciones { get; set; }
    }
}
