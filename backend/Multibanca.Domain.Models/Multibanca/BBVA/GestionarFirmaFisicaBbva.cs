using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

/// <summary>
/// Entidad que representa el registro de Gestionar Firma Física en BBVA.
/// Almacena información de motorizado asignado, fecha de gestoría y resultado.
/// </summary>
public class gestionar_firma_fisica_bbva : base_auditoria
{
    public int id { get; set; }
    public long id_expediente { get; set; }
    public string? id_actividad { get; set; } = "ACT_FIRMA_FISICA";
    public string? motorizado_asignado { get; set; }
    public DateTime? fecha_gestoria { get; set; }
    public string? resultado_gestoria { get; set; }   // Catálogo L10
    public string? observaciones { get; set; }
}
