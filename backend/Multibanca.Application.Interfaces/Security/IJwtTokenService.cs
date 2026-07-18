using Multibanca.DTO.Multibanca;

namespace Multibanca.Application.Interfaces.Security;

/// <summary>
/// Servicio reutilizable para emitir JWTs usados por funcionalidades de Multibanca.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Valida que exista la configuracion necesaria para emitir JWTs temporales.
    /// </summary>
    void ValidateTemporalJwtConfiguration();

    /// <summary>
    /// Emite un JWT temporal con claims minimos para navegar como usuario externo.
    /// </summary>
    /// <param name="token">Datos de contexto obtenidos al consumir el token temporal.</param>
    /// <returns>JWT firmado para la sesion temporal.</returns>
    string BuildTemporalJwt(AccesoTemporalTokenDTO token);
}
