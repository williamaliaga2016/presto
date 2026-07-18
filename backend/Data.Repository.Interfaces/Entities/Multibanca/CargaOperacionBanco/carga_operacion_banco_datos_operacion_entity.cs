using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_datos_operacion_entity
    {
        [Key]
        public int id_carga_operacion_banco_datos_operacion { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public long? nro_mutuo { get; set; }

        public string? tipo_operacion { get; set; }

        public long? nro_registro { get; set; }

        public string? ult_clasif_al { get; set; }

        public string? segmento { get; set; }

        public string? canal_venta { get; set; }

        public long? nro_op_cartera { get; set; }

        public string? modelo_operacion { get; set; }

        public string? tipo_carpeta { get; set; }

        public string? propietario { get; set; }

        public string? inmobiliaria { get; set; }

        public string? glosa_producto { get; set; }

        public string? codigo_producto_comercial { get; set; }

        public int? nro_piloto { get; set; }

        public string? banco_alzante { get; set; }

        public string? nombre_proyecto { get; set; }

        // BBVA COLOMBIA
        public string? id_scoring { get; set; }
        public string? codigo_asesor { get; set; }
        public string? codigo_oficina { get; set; }
        public string? descripcion_oficina { get; set; }
        public string? canal_originacion { get; set; }
        public string? tipo_inmueble { get; set; }
        public string? estado_inmueble { get; set; }
        public string? descripcion_estado_inmueble { get; set; }
        public string? codigo_proyecto { get; set; }
        public string? descripcion_proyecto { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
