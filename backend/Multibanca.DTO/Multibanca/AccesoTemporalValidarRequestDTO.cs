namespace Multibanca.DTO.Multibanca;

/// <summary>
/// Solicitud publica para validar un token recibido desde el link de acceso temporal.
/// </summary>
public class AccesoTemporalValidarRequestDTO
{
    public string token { get; set; } = string.Empty;
}
