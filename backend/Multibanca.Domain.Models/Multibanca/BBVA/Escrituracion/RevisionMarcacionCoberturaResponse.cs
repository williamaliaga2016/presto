namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

/// <summary>
/// Respuesta del endpoint GET /api/RevisionMarcacionCobertura/GetByExpediente/{id}.
/// </summary>
public class RevisionMarcacionCoberturaResponse
{
    /// <summary>Datos editables del formulario.</summary>
    public revision_marcacion_cobertura_bbva formulario { get; set; } = null!;

    /// <summary>
    /// Datos generales de la solicitud (CA02), obtenidos del encabezado del expediente.
    /// No existe una tabla predecesora real (BBV-104 "Validar Cumplimiento de Políticas" aún no
    /// está construida), por lo que la herencia se limita a la información general del caso.
    /// </summary>
    public RevisionMarcacionCoberturaHerencia? herencia { get; set; }
}

/// <summary>
/// Datos heredados de solo lectura (CA02), derivados del encabezado del expediente.
/// </summary>
public class RevisionMarcacionCoberturaHerencia
{
    public string? nombre_cliente { get; set; }
    public string? numero_identificacion { get; set; }
    public string? tipo_identificacion { get; set; }
}
