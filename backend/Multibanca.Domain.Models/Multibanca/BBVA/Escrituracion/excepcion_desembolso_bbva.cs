using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class excepcion_desembolso_bbva : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO";

    // Campos de decisión (editables)
    public string? excepcion_autorizada { get; set; }       // "SI" | "NO" | null
    public string? requiere_vobo_gerencia { get; set; }     // "SI" | "NO" | null
    public bool confirmacion_excepcion { get; set; }         // Checkbox obligatorio para avanzar

    // Campo opcional
    public string? observaciones_excepcion { get; set; }     // max 1000 chars

    // Origen del caso (informativo, determinado automáticamente)
    public string? origen_caso { get; set; }                 // "FIRMAR_REP_LEGAL" | "RECEPCION_BOLETA"
}
