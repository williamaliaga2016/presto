namespace Multibanca.DTO.Multibanca.BBVA;

/// <summary>
/// Resultado del avance de Asignar Firmas, incluyendo el workflow ejecutado y el link temporal cuando aplica.
/// </summary>
public class AsignarFirmasAvanzarResponseDTO
{
    /// <summary>
    /// Actividades asignadas por el motor de workflow despues de ejecutar la transicion.
    /// </summary>
    public object? workflow { get; set; }

    /// <summary>
    /// Link temporal generado para el cliente externo cuando se requiere notificacion.
    /// </summary>
    public AccesoTemporalGenerarResponseDTO? acceso_temporal { get; set; }
}
