using Multibanca.Common;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Extensions;

/// <summary>
/// Expone utilidades para leer claims de usuarios autenticados desde controllers.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Obtiene el identificador interno del usuario autenticado desde los claims soportados por la API.
    /// </summary>
    /// <param name="user">Principal autenticado recibido por el controller.</param>
    /// <returns>Identificador interno del usuario autenticado.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando el claim no existe o no es numerico.</exception>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        string? userIdClaim = user.Claims.FirstOrDefault(q =>
            q.Type == "user_id" || q.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }

    /// <summary>
    /// Obtiene el codigo de rol (performer, igual a role.code) del usuario autenticado desde los claims soportados por la API.
    /// </summary>
    /// <param name="user">Principal autenticado recibido por el controller.</param>
    /// <returns>Codigo de rol (performer) del usuario autenticado.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando el claim no existe.</exception>
    public static string GetPerformerId(this ClaimsPrincipal user)
    {
        // No se usa ClaimTypes.Role como fallback: ese claim guarda role_name (ej. "Coordinador"),
        // no role.code (ej. "COORDINADOR"), que es contra lo que compara usp_select_actividad_asignar.
        string? performerClaim = user.Claims.FirstOrDefault(q => q.Type == "performer")?.Value;

        if (string.IsNullOrEmpty(performerClaim))
            throw new InvalidOperationException("No fue posible identificar el rol (performer) del usuario autenticado.");

        return performerClaim;
    }

    /// <summary>
    /// Indica si el JWT autenticado corresponde a una sesion temporal de usuario externo.
    /// </summary>
    /// <param name="user">Principal autenticado recibido por el controller.</param>
    /// <returns>True cuando el claim de tipo de acceso identifica una sesion temporal.</returns>
    public static bool IsTemporalAccess(this ClaimsPrincipal user)
    {
        string? accessType = user.Claims.FirstOrDefault(q =>
            q.Type == Constants.AccesoTemporal.ClaimTipoAcceso)?.Value;

        return accessType == Constants.AccesoTemporal.TipoAccesoTemporal;
    }

    /// <summary>
    /// Obtiene el UUID del token temporal autenticado cuando el request proviene de un link externo.
    /// </summary>
    /// <param name="user">Principal autenticado recibido por el controller.</param>
    /// <returns>UUID del token temporal o `null` cuando el usuario no usa acceso temporal.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando la sesion temporal no contiene un token UUID valido.</exception>
    public static Guid? GetTemporalAccessToken(this ClaimsPrincipal user)
    {
        if (!user.IsTemporalAccess())
            return null;

        string? tokenClaim = user.Claims.FirstOrDefault(q =>
            q.Type == Constants.AccesoTemporal.ClaimToken)?.Value;

        if (!Guid.TryParse(tokenClaim, out Guid token))
            throw new InvalidOperationException("No fue posible identificar el token de acceso temporal.");

        return token;
    }
}
