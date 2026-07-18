using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.Entities.Multibanca.ValidacionRectificatoriaLegalPostventa
{
    public class validacion_rectificatoria_legal_postventa_datos_personales_entity
    {

        [Key]
        public int id_validacion_rectificatoria_legal_postventa_datos_personales { get; set; }

        public int id_validacion_rectificatoria_legal_postventa { get; set; }

        public long id_expediente { get; set; }
        public string? rut { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public string? genero { get; set; }

        public string? nombres { get; set; }

        public string? apellido_paterno { get; set; }

        public string? apellido_materno { get; set; }

        public string? nacionalidad { get; set; }
        public string? relacion_titular { get; set; }

        public string? profesion { get; set; }
        public string? direccion { get; set; }

        public string? estado_civil { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? rol_comparecencia { get; set; }

        public bool is_active { get; set; }

        public bool row_status { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_date { get; set; }
    }
}
