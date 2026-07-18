using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

/// <summary>
/// Registro persistente de la confirmacion de soportes de pago cargados por el cliente.
/// </summary>
public class cargar_soportes_pago : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "BBVA_CONTACTO_CARGAR_SOPORTES_DE_PAGO_899F408B";
    public bool documentos_adjuntos { get; set; }
    public string? observaciones { get; set; }
}
