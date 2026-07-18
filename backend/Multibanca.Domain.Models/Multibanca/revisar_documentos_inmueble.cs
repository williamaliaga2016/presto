using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class revisar_documentos_inmueble : base_auditoria
    {
        public int id { get; set; }
        public long id_expediente { get; set; }
        public string id_actividad { get; set; } = "ACT_REVISAR_DOCS";
        public bool? documentos_correctos { get; set; }
        public string? motivo_devolucion { get; set; }
        public string? observaciones { get; set; }
        // "Si"/"No" como texto (no booleano), segun definicion de negocio.
        public string? requiere_actualizacion_avaluos { get; set; }
        public string? homologacion { get; set; }
    }
}
