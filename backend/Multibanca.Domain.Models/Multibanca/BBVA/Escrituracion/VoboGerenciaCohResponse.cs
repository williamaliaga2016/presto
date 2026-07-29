namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

/// <summary>
/// Respuesta del endpoint GET /api/VoboGerenciaCoh/GetByExpediente/{id}.
/// </summary>
public class VoboGerenciaCohResponse
{
    /// <summary>Datos editables del formulario.</summary>
    public vobo_gerencia_coh_bbva formulario { get; set; } = null!;

    /// <summary>Datos heredados de Excepcion Desembolso (solo lectura).</summary>
    public VoboGerenciaCohHerencia? herencia { get; set; }
}

/// <summary>
/// Campos heredados de la actividad Excepcion Desembolso (solo lectura).
/// </summary>
public class VoboGerenciaCohHerencia
{
    // Datos de cliente y notaría (desde encabezado)
    public string? nombre_cliente { get; set; }
    public string? numero_identificacion { get; set; }
    public string? tipo_identificacion { get; set; }
    public string? nombre_notaria { get; set; }
    public string? ciudad_notaria { get; set; }

    // Datos del formulario previo (desde excepcion_desembolso)
    public string? excepcion_autorizada { get; set; }       // "SI" | "NO" | null
    public string? requiere_vobo_gerencia { get; set; }     // "SI" | "NO" | null
    public bool? confirmacion_excepcion { get; set; }
    public string? observaciones_excepcion { get; set; }
}
