using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class asignar_escritura : base_auditoria
    {
        public int id_asignar_escritura { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = string.Empty;
        public string abogado { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
    }
}
