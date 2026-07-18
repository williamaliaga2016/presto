using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

/// <summary>
/// Registro persistente de la confirmacion documental realizada en la actividad Cargar Documentos Cliente.
/// </summary>
public class cargar_documentos_cliente : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738";
    public bool documentos_adjuntos { get; set; }
    public string? observaciones { get; set; }
}
