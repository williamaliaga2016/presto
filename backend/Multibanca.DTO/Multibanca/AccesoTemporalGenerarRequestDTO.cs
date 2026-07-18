namespace Multibanca.DTO.Multibanca;

/// <summary>
/// Solicitud autorizada para crear un link temporal asociado a un expediente y actividad.
/// </summary>
public class AccesoTemporalGenerarRequestDTO
{
    public long id_expediente { get; set; }
    public int id_usuario_cliente { get; set; }
    public string id_actividad { get; set; } = string.Empty;
    public int? horas_expiracion { get; set; }
}
