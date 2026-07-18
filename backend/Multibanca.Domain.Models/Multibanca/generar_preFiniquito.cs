using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_prefiniquito: base_auditoria
    {
        public int id_generar_prefiniquito {  get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
    }
}
