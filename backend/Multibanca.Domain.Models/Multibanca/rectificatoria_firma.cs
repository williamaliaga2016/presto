using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca
{
    public class rectificatoria_firma : base_auditoria
    {
        public int id_rectificatoria_firma { get; set; }

        public long id_expediente { get; set; }

        public int id_usuario_solicitante { get; set; }

        public bool is_subsanar { get; set; }

        public string? observaciones { get; set; }

        // Campos enriquecidos para pantalla. No existen en la tabla física.
        public string? solicitante { get; set; }

        public string? observaciones_reparo { get; set; }

        public DateTime? fecha_ingreso { get; set; }

        public List<rectificatoria_firma_detalle>? detalles { get; set; }
    }
}
