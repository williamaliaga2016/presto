using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class asignar_estudio_titulos : base_auditoria
    {
        public int id_asignar_estudio_titulos { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = string.Empty;
        public string abogado { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
    }
}

