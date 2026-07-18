using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class generar_estudio_titulos:base_auditoria
    {
        public int id_generar_estudio_titulos { get; set; }
        public long id_expediente { get; set; }
        public string? observaciones { get; set; }
        public bool enviar_reparo { get; set; }
    }
}
