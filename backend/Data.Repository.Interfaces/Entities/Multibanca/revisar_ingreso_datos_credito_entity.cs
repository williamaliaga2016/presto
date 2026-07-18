using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca
{
    public class revisar_ingreso_datos_credito_entity
    {
        [Key]
        public int id_revisar_ingreso_datos_credito { get; set; }

        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public bool? ubicacion { get; set; }

        public string? tipo_operacion { get; set; }

        public bool? fines_generales { get; set; }

        public string? nombre_proyecto { get; set; }

        public bool? credito_segunda_vivienda { get; set; }

        public string? inmobiliaria { get; set; }

        public bool? vivienda_social { get; set; }

        public bool? dfl2 { get; set; }

        public bool? propietario_dfl2 { get; set; }

        public bool? recepcion_final_mayor_2_anios { get; set; }

        public decimal? porcentaje_impuesto { get; set; }

        public decimal? monto_credito_afecto_impuesto { get; set; }

        public decimal? impuesto_a_pagar { get; set; }

        public bool? enviar_a_reparo { get; set; }

        public string? observaciones { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
