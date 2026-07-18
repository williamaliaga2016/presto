using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class aprobacion_comercial_legal_cdr : base_auditoria
    {
        public int id_aprobacion_comercial_legal_cdr { get; set; }
        public long id_expediente { get; set; }
        public bool enviar_a_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
