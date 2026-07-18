using Common.Domain.Models;
using System;

namespace Multibanca.Domain.Models.Multibanca
{
    public class revisar_inscripcion_cbr : base_auditoria
    {
        public int id_revisar_inscripcion_cbr { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool is_enviar_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
