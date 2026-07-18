using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class corregir_reparo_generar_borrador_escritura : base_auditoria
    {
        public int id_corregir_reparo_generar_borrador_escritura { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public string? observaciones { get; set; }

        public string? solicitante { get; set; }

        public string? observaciones_reparo { get; set; }

        public DateTime? fecha_ingreso { get; set; }
    }
}
