using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.RevisarDatosOperacion
{
    public class revisar_datos_operacion : base_auditoria
    {
        public int id_revisar_datos_operacion { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
        public bool enviar_reparo { get; set; }

        public revisar_datos_operacion_credito? credito { get; set; }

        public revisar_datos_operacion_propiedad? propiedad { get; set; }

        public revisar_datos_operacion_banco? revisar_datos_operacion_banco { get; set; }

        public List<revisar_datos_operacion_comprador>? compradores { get; set; }

        public List<revisar_datos_operacion_fiador_garante>? fiadores_garantes { get; set; }
    }
}
