namespace Multibanca.DTO.Multibanca;

/// <summary>
/// Datos minimos del token consumido que se usan para construir el JWT temporal.
/// </summary>
public class AccesoTemporalTokenDTO
{
    public Guid token { get; set; }
    public int id_usuario { get; set; }
    public long id_expediente { get; set; }
    public string id_actividad { get; set; } = string.Empty;
}
