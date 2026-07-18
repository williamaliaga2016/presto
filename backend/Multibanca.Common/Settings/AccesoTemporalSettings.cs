namespace Multibanca.Common.Settings;

/// <summary>
/// Configuracion no sensible para generar links y JWTs de acceso temporal.
/// </summary>
public class AccesoTemporalSettings
{
    public int HorasExpiracion { get; set; } = 72;
    public int JwtHorasExpiracion { get; set; } = 4;
    public string FrontendBaseUrl { get; set; } = "http://localhost:5173";
}
