using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.DatosOperacion
{
    public class datos_operacion_fiador_garante : base_auditoria
    {
        public int id_datos_operacion_fiador_garante { get; set; }

        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public string? rut { get; set; }

        public string? nombres { get; set; }

        public string? apellido_paterno { get; set; }

        public string? apellido_materno { get; set; }

        public DateTime? fecha_nacimiento { get; set; }

        public string? genero { get; set; }

        public string? estado_civil { get; set; }

        public string? nacionalidad { get; set; }

        public string? profesion { get; set; }

        public string? direccion { get; set; }

        public string? region { get; set; }

        public string? comuna { get; set; }

        public string? telefono_fijo { get; set; }

        public string? telefono_movil { get; set; }

        public string? email { get; set; }

        public string? relacion_titular { get; set; }
    }
}
