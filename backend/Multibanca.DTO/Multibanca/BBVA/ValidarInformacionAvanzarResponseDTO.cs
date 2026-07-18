namespace Multibanca.DTO.Multibanca.BBVA;

/// <summary>
/// Resultado del avance de Validar Informacion, incluyendo el workflow ejecutado y el link temporal cuando aplica.
/// </summary>
public class ValidarInformacionAvanzarResponseDTO
{
    /// <summary>
    /// Actividades asignadas por el motor de workflow despues de ejecutar la transicion.
    /// </summary>
    public object? workflow { get; set; }

    /// <summary>
    /// Link temporal generado para el cliente externo cuando la decision requiere carga documental.
    /// </summary>
    public AccesoTemporalGenerarResponseDTO? acceso_temporal { get; set; }
}
