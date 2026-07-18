using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_datos_comercial_entity
    {
        [Key]
        public int id_carga_operacion_banco_datos_comercial { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public long? codigo_ejecutivo { get; set; }

        public string? login_ejecutivo { get; set; }

        public string? nombre_ejecutivo { get; set; }

        public long? rut_ejecutivo { get; set; }

        public long? codigo_oficina { get; set; }

        public string? nombre_oficina { get; set; }

        public long? codigo_curse { get; set; }

        public string? glosa_curse { get; set; }

        public long? codigo_ejecutivo_curse { get; set; }

        public string? login_ejecutivo_curse { get; set; }

        public string? nombre_ejecutivo_curse { get; set; }

        public long? rut_ejecutivo_curse { get; set; }

        public long? rut_banco { get; set; }

        public string? renovacion_urbana { get; set; }

        public string? nombre_banco { get; set; }

        public string? tipo_hipoteca { get; set; }

        // BBVA COLOMBIA
        public string? correo_declarativo_cliente { get; set; }
        public string? numero_telefono_declarativo { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
