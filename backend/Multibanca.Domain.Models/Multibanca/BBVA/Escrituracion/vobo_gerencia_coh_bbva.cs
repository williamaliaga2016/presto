namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class vobo_gerencia_coh_bbva
{
    public long id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = "BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH";

    // Campos de dictamen (editables por el Gerente COH)
    public string? concepto_vobo { get; set; }    // "Favorable" | "No Favorable" | null
    public string? observaciones { get; set; }    // max 1000 chars

    // Campos de auditoría (explícitos, sin heredar de base_auditoria)
    public bool is_active { get; set; }
    public bool row_status { get; set; }
    public int created_by { get; set; }
    public DateTime created_date { get; set; }
    public int? modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
