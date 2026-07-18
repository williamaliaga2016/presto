using Multibanca.DTO.Multibanca;

namespace Multibanca.Application.Interfaces.Multibanca;

/// <summary>
/// Servicio de aplicacion para generar y validar accesos temporales de usuarios externos.
/// </summary>
public interface IAccesoTemporalApplication
{
    /// <summary>
    /// Crea un token temporal asociado a un expediente, actividad y usuario cliente.
    /// </summary>
    /// <param name="request">Datos requeridos para generar el acceso temporal.</param>
    /// <param name="userId">Usuario autenticado que solicita la generacion del link.</param>
    /// <returns>Token generado, fecha de expiracion y URL publica para el frontend.</returns>
    Task<AccesoTemporalGenerarResponseDTO> Generar(
        AccesoTemporalGenerarRequestDTO request,
        int userId);

    /// <summary>
    /// Valida un token temporal publico y emite un JWT temporal para el cliente externo.
    /// </summary>
    /// <param name="request">Token recibido desde la ruta publica de acceso temporal.</param>
    /// <returns>JWT temporal, datos de contexto y ruta de redireccion.</returns>
    Task<AccesoTemporalValidarResponseDTO> Validar(AccesoTemporalValidarRequestDTO request);

    /// <summary>
    /// Marca como usado el token temporal cuando la accion externa finaliza correctamente.
    /// </summary>
    /// <param name="token">UUID del token temporal autenticado.</param>
    /// <param name="idExpediente">Expediente asociado al token.</param>
    /// <param name="idActividad">Actividad asociada al token.</param>
    Task ConsumirDespuesDeAccionExitosa(Guid token, long idExpediente, string idActividad);
}
