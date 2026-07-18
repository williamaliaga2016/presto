using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class reparo_estudio_titulos_detalle : base_auditoria
    {
        public int id_reparo_estudio_titulos { get; set; }
        public long id_expediente { get; set; }
        public string? solicitante { get; set; }
        public string? observaciones { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public bool subsanar { get; set; }
    }
}
