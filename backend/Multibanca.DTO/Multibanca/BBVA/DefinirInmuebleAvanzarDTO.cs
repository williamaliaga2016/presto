namespace Multibanca.DTO.Multibanca.BBVA;

/// <summary>
/// Solicitud de avance de la actividad BBV-44 Definir Inmueble.
/// </summary>
public class DefinirInmuebleAvanzarRequestDTO
{
    /// <summary>
    /// Confirma el avance cuando el inmueble sigue sin definirse.
    /// </summary>
    public bool confirmar { get; set; }
}

/// <summary>
/// Resultado del avance de la actividad BBV-44 Definir Inmueble.
/// </summary>
public class DefinirInmuebleAvanzarResponseDTO
{
    /// <summary>
    /// Indica que el frontend debe mostrar confirmacion antes de ejecutar el avance.
    /// </summary>
    public bool requiere_confirmacion { get; set; }

    /// <summary>
    /// Mensaje de confirmacion o resultado.
    /// </summary>
    public string? mensaje { get; set; }

    /// <summary>
    /// Actividades asignadas por el motor de workflow despues de ejecutar la transicion.
    /// </summary>
    public object? workflow { get; set; }
}
