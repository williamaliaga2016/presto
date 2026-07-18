using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion
{
    public class datos_operacion_propiedad_entity
    {
        [Key]
        public int id_datos_operacion_propiedad { get; set; }

        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public string? tipo_propiedad { get; set; }

        public string? estado { get; set; }

        public string? tipo_venta { get; set; }

        public string? tipo_construccion { get; set; }

        public string? tipo_direccion { get; set; }

        public string? direccion { get; set; }

        public string? villa_condominio { get; set; }

        public string? numero { get; set; }

        public string? numero_casa_habitantes { get; set; }

        public string? conjunto { get; set; }

        public string? manzana { get; set; }

        public string? lote { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? existe_rol_avaluo { get; set; }

        public string? rol_avaluo_1 { get; set; }

        public string? rol_avaluo_2 { get; set; }

        public decimal? valor_avaluo_pesos { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
