using System.Text.Json.Serialization;

namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

/// <summary>
/// Respuesta tipada del endpoint GET /api/excepcion-desembolso/{idExpediente}.
/// Contiene el formulario editable, los datos heredados de la actividad previa
/// y el origen determinado automáticamente por el sistema.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrigenCasoEnum
{
    FIRMAR_REP_LEGAL,
    RECEPCION_BOLETA
}

public class ExcepcionDesembolsoResponse
{
    /// <summary>
    /// Datos editables de la actividad. Si no existe registro previo,
    /// contiene un modelo vacío con id_expediente precargado.
    /// </summary>
    public excepcion_desembolso_bbva formulario { get; set; } = null!;

    /// <summary>
    /// Campos heredados de solo lectura. Null si el origen no pudo determinarse.
    /// </summary>
    public ExcepcionDesembolsoHerencia? herencia { get; set; }

    /// <summary>
    /// Origen determinado automáticamente: "FIRMAR_REP_LEGAL" | "RECEPCION_BOLETA" | null.
    /// </summary>
    [JsonIgnore]
    public OrigenCasoEnum? _OrigenCaso { get; set; }

    public string? origenCaso => OrigenCasoEnum.FIRMAR_REP_LEGAL.Equals(_OrigenCaso) 
        ? "Firmar Representante Legal"
        : _OrigenCaso == null 
            ? "-"
            : "Realizar Recepción Boleta";

    public Boolean esRecepcionBoleta => OrigenCasoEnum.RECEPCION_BOLETA.Equals(_OrigenCaso);
}

/// <summary>
/// Campos heredados comunes a ambos orígenes (Firmar Rep. Legal y Recepción Boleta).
/// </summary>
public class ExcepcionDesembolsoHerencia
{
    /// <summary>Concepto de firma registrado en la actividad Firmar Rep. Legal.</summary>
    public string? conceptoFirma { get; set; }

    public string? conceptoFirmaDesc { get; set; }

    // ── Campos exclusivos de Recepción Boleta ────────────────────────────────

    /// <summary>Fecha de ingreso a la oficina de registro. Solo presente si origen = RECEPCION_BOLETA.</summary>
    public DateTime? fechaBoleta { get; set; }

    /// <summary>Número de radicado / boleta. Solo presente si origen = RECEPCION_BOLETA.</summary>
    public string? numeroBoleta { get; set; }

    /// <summary>Tipo de boleta. Solo presente si origen = RECEPCION_BOLETA.</summary>
    public string? tipoBoleta { get; set; }

    /// <summary>Oficina de registro. Solo presente si origen = RECEPCION_BOLETA.</summary>
    public string? oficinaRegistro { get; set; }
}
