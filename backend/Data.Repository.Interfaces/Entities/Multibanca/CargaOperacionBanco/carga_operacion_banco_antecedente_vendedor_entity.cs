using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.CargaOperacionBanco
{
    public class carga_operacion_banco_antecedente_vendedor_entity
    {
        [Key]
        public int id_carga_operacion_banco_antecedente_vendedor { get; set; }

        public int id_carga_operacion_banco { get; set; }

        public long id_expediente { get; set; }

        public string? rut { get; set; }

        public string? tipo_vendedor { get; set; }

        public string? razon_social { get; set; }

        public string? nombres { get; set; }

        public string? apellido_paterno { get; set; }

        public string? apellido_materno { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        public string? genero { get; set; }

        public string? estado_civil { get; set; }

        public string? relacion_titular { get; set; }

        public string? direccion { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? telefono { get; set; }

        public string? email { get; set; }

        public string? nacionalidad { get; set; }

        public string? profesion { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
