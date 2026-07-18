using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class revisar_desembolso : base_auditoria
    {
        public int id_revisar_desembolso {  get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
