using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class corregir_reparo_inst_pago : base_auditoria
    {
        public int id_corregir_reparo_inst_pago { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public bool enviar_a_reparo { get; set; }

        public string? observaciones { get; set; }

        public string? solicitante { get; set; }

        public string? observaciones_reparo { get; set; }

        public DateTime? fecha_ingreso { get; set; }
    }
}
