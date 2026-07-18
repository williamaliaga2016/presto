using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class firma_banco_acreedor_cg : base_auditoria
    {
        public int id_firma_banco_acreedor_cg { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
