using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Repository.Interfaces.Entities.Multibanca.DatosOperacion
{
    public class datos_operacion_comprador_entity
    {
        [Key]
        public int id_datos_operacion_comprador { get; set; }

        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public string? rut { get; set; }

        public string? tipo_persona { get; set; }

        public string? razon_social { get; set; }

        public string? nombres { get; set; }

        public string? apellido_paterno { get; set; }

        public string? apellido_materno { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        public string? genero { get; set; }

        public string? estado_civil { get; set; }

        public string? nacionalidad { get; set; }

        public string? profesion { get; set; }

        public string? relacion_titular { get; set; }

        public string? direccion { get; set; }

        public string? direccion_env_esc { get; set; }

        public string? region { get; set; }

        public string? region_env_esc { get; set; }

        public string? comuna { get; set; }

        public string? comuna_env_esc { get; set; }

        public string? direccion_env_div { get; set; }

        public string? region_env_div { get; set; }

        public string? comuna_env_div { get; set; }

        public int? tipo_dir_dividendo { get; set; }

        public string? telefono { get; set; }

        public string? telefono_comercial { get; set; }

        public string? telefono_movil { get; set; }

        public string? email { get; set; }

        public string? email2 { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
