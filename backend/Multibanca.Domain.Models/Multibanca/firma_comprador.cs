using Common.Domain.Models;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;

namespace Multibanca.Domain.Models.Multibanca
{
    public class firma_comprador : base_auditoria
    {
        public int id_firma_comprador { get; set; }
        public long id_expediente { get; set; }
        public string observaciones { get; set; } = string.Empty;
        public List<firma_comprador_detalle>? firma_comprador_detalle { get; set; }
    }
}
