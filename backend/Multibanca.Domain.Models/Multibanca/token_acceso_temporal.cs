namespace Multibanca.Domain.Models.Multibanca;

/// <summary>
/// Registro de un link temporal de un solo uso para autenticar usuarios externos sin exponer datos sensibles.
/// </summary>
public class token_acceso_temporal
{
    public int id { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = string.Empty;
    public int id_usuario { get; set; }
    public Guid token { get; set; }
    public DateTime fecha_expiracion { get; set; }
    public bool usado { get; set; }
    public DateTime? fecha_uso { get; set; }
    public bool is_active { get; set; }
    public int created_by { get; set; }
    public DateTime created_date { get; set; }
}
