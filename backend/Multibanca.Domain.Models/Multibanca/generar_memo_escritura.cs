using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_memo_escritura : base_auditoria
    {
        public int id_generar_memo_escritura { get; set; }
        public long id_expediente { get; set; }
        public bool enviar_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
