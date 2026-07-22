using Common.Domain.Models;

namespace Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;

public class firmar_escritura_cliente_bbva
{
    public long     id { get; set; }
    public long     id_expediente { get; set; }
    public string   id_actividad { get; set; } = "BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F";

    // Bloque Información de Notaría (heredado, editable)
    public string?   notaria { get; set; }
    public DateTime? fecha_notaria { get; set; }
    public int?      numero_notaria { get; set; }
    public string?   ciudad_notaria { get; set; }

    // Bloque Formalización de Escritura
    public string?   numero_escritura { get; set; }
    public DateTime? fecha_escritura { get; set; }
    public string?   representante_legal { get; set; }

    // Decisiones de Enrutamiento
    public string? requiere_escalamiento_comercial { get; set; } // "SI" | "NO" | null
    public string? tipologia { get; set; }
    public string? requiere_causar { get; set; } // "SI" | "NO" | null (solo Leasing)

    // Campos adicionales
    public string? observaciones { get; set; }
    public string? tipo_credito { get; set; }

    // Auditoria (siempre crear, no implementar de base_auditoria)
    public bool      is_active { get; set; }
    public bool      row_status { get; set; }
    public int       created_by { get; set; }
    public DateTime  created_date { get; set; }
    public int?      modified_by { get; set; }
    public DateTime? modified_date { get; set; }
}
