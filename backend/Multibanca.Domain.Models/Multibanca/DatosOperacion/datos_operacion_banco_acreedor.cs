using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.DatosOperacion
{
    public class datos_operacion_banco_acreedor : base_auditoria
    {
        public int id_datos_operacion_banco_acreedor { get; set; }

        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }

        public bool? cuenta_carta_resguardo { get; set; }

        public string? institucion { get; set; }

        public string? rut_banco_acreedor { get; set; }
    }
}
