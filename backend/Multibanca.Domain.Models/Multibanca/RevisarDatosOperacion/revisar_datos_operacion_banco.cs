using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion
{
    public class revisar_datos_operacion_banco:base_auditoria
    {
        public int id_revisar_datos_operacion_banco { get; set; }
        public int id_revisar_datos_operacion { get; set; }
        public long id_expediente { get; set; }
        public bool? cuenta_carta_resguardo { get; set; }
        public string? institucion { get; set; }
        public string? rut_banco_acreedor { get; set; }
        public bool? enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
