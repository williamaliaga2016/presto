using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class control_escritura : base_auditoria
    {
        public int id_control_escritura { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public bool is_enviar_reparo { get; set; }
        public string? observaciones { get; set; }
    }
}
