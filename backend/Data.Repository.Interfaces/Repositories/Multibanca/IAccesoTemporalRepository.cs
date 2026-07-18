using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca;

namespace Data.Repository.Interfaces.Repositories.Multibanca;

/// <summary>
/// Persistencia del token temporal de un solo uso.
/// </summary>
public interface IAccesoTemporalRepository
{
    /// <summary>
    /// Crea un registro de token temporal activo y pendiente de consumo.
    /// </summary>
    /// <param name="request">Entidad de token temporal a persistir.</param>
    /// <returns>Entidad persistida con valores generados.</returns>
    Task<token_acceso_temporal> Crear(token_acceso_temporal request);

    /// <summary>
    /// Obtiene un token disponible sin marcarlo como usado.
    /// </summary>
    /// <param name="token">UUID del token recibido desde el link temporal.</param>
    /// <param name="fechaActual">Fecha usada para validar expiracion.</param>
    /// <returns>Datos minimos para emitir el JWT temporal.</returns>
    Task<AccesoTemporalTokenDTO> ObtenerTokenDisponible(Guid token, DateTime fechaActual);

    /// <summary>
    /// Consume el token asociado al contexto temporal despues de completar la accion externa.
    /// </summary>
    /// <param name="token">UUID del token temporal autenticado.</param>
    /// <param name="idExpediente">Expediente asociado al token temporal.</param>
    /// <param name="idActividad">Actividad asociada al token temporal.</param>
    /// <param name="fechaActual">Fecha usada para validar expiracion y registrar consumo.</param>
    Task ConsumirToken(Guid token, long idExpediente, string idActividad, DateTime fechaActual);
}
