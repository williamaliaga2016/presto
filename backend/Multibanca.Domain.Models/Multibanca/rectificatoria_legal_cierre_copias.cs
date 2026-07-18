using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class rectificatoria_legal_cierre_copias : base_auditoria
    {
        public int id_rectificatoria_legal_cierre_copias { get; set; }
        public long id_expediente { get; set; }
        public int id_usuario_solicitante { get; set; }
        public DateTime? fecha_firma { get; set; }
        public string? observaciones { get; set; }
        public string? nombre_banco_alzante { get; set; }
    }
}
