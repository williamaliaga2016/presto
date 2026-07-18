using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class firma_vendedor : base_auditoria
    {
        public int id_firma_vendedor { get; set; }
        public long id_expediente { get; set; }
        public string observaciones { get; set; } = string.Empty;
        public List<firma_vendedor_detalle>? firma_vendedor_detalle { get; set; }
    }
}
