using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.DatosOperacion
{
    public class datos_operacion : base_auditoria
    {
        public int id_datos_operacion { get; set; }

        public long id_expediente { get; set; }
        public bool? enviar_reparo { get; set; }

        public string? observaciones { get; set; }

        // ============================================================
        // TABS / SECCIONES HIJAS DE LA ACTIVIDAD 5.4
        // ============================================================

        public datos_operacion_datos_credito? datos_credito { get; set; }

        public List<datos_operacion_comprador>? compradores { get; set; }

        public List<datos_operacion_vendedor>? vendedores { get; set; }

        public List<datos_operacion_fiador_garante>? fiadores_garantes { get; set; }

        public datos_operacion_banco_acreedor? banco_acreedor { get; set; }

        public datos_operacion_propiedad? propiedad { get; set; }
    }
}
