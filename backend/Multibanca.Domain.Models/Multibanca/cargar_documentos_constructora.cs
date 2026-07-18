using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class cargar_documentos_constructora : base_auditoria
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_DOCS_CONSTRUCTORA";
        public bool avanzar_validar_documentos { get; set; }
        public string observaciones { get; set; } = string.Empty;
    }
}
