using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class rectificatoria_postventa_solucion_reparo : base_auditoria
    {
        public int id_rectificatoria_postventa_solucion_reparo { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }
        public bool modificar_datos_memo { get; set; }
        public bool descontabilizar_operacion { get; set; }

        public string? observaciones { get; set; }

        public string? solicitante { get; set; }

        public string? observaciones_reparo { get; set; }

        public DateTime? fecha_ingreso { get; set; }
    }
}
