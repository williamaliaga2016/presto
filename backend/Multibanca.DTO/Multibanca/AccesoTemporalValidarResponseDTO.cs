namespace Multibanca.DTO.Multibanca;

/// <summary>
/// Respuesta de validacion exitosa con JWT temporal y datos minimos para navegar a la actividad.
/// </summary>
public class AccesoTemporalValidarResponseDTO
{
    public string jwt { get; set; } = string.Empty;
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = string.Empty;
    public string url_redirect { get; set; } = string.Empty;
}
