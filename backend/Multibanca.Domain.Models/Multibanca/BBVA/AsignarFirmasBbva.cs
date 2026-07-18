using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA;

public class asignar_firmas_peritos_abogados : base_auditoria
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "ACT_ASIGNAR_FIRMAS";
    public string? tipo_cliente { get; set; }
    public string? codigo_ejecutivo_solicitante { get; set; }
    public string? oficina_solicitante { get; set; }
    public string? tipo_solicitud_avaluo { get; set; }
    public string? tipo_tramite_eett { get; set; }
    public string? nombre_firma_supervisor { get; set; }
    public string? telefono_firma { get; set; }
    public string? email_firma { get; set; }
    public decimal? valor_avaluo { get; set; }
    public decimal? valor_total_consignar { get; set; }
    public string? opciones_recaudo { get; set; }
    public string? numero_recaudo { get; set; }
    public string? banco { get; set; }
    public string? nombre_abogado { get; set; }
    public string? telefono_abogado { get; set; }
    public decimal? valor_estudio_titulos { get; set; }
    public string? tipo_cuenta_abogado { get; set; }
    public string? numero_cuenta_abogado { get; set; }
    public bool? requiere_envio_notificacion { get; set; }
    public string? checklist_documentos_solicitar { get; set; }
    public string? observaciones { get; set; }
}

public record CalcularAsignacionRequest(
    string tipo_cliente,
    string codigo_ejecutivo,
    string oficina,
    string tipo_solicitud_avaluo,
    string tipo_tramite_eett,
    long id_expediente);
