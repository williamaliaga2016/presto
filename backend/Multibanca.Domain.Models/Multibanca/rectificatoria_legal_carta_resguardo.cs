using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class rectificatoria_legal_carta_resguardo : base_auditoria
    {
        public int id_rectificatoria_legal_carta_resguardo { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public string? observaciones { get; set; }
    }
}
