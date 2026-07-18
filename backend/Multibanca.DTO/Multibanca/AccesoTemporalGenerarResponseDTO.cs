namespace Multibanca.DTO.Multibanca;

/// <summary>
/// Resultado de generacion del link temporal que puede ser enviado al cliente externo.
/// </summary>
public class AccesoTemporalGenerarResponseDTO
{
    public string url { get; set; } = string.Empty;
    public Guid token { get; set; }
    public DateTime fecha_expiracion { get; set; }
}
