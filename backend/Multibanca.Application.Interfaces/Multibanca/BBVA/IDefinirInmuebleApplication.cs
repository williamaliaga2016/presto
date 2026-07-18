using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Application.Interfaces.Multibanca.BBVA;

/// <summary>
/// Contrato de aplicacion para BBV-44 Definir Inmueble.
/// </summary>
public interface IDefinirInmuebleApplication
{
    /// <summary>
    /// Consulta formulario, datos heredados y encabezado de la actividad.
    /// </summary>
    Task<object> GetConEncabezado(long idExpediente);

    /// <summary>
    /// Consulta catalogos requeridos por la pantalla.
    /// </summary>
    Task<object> GetControles();

    /// <summary>
    /// Guarda los datos propios de la actividad.
    /// </summary>
    Task<definir_inmueble_bbva> Guardar(definir_inmueble_bbva request, int userId);

    /// <summary>
    /// Avanza la actividad por la transicion de workflow confirmada.
    /// </summary>
    Task<DefinirInmuebleAvanzarResponseDTO> Avanzar(long idExpediente, int userId, bool confirmar);
}
