using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.DTO.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca;

[ApiController]
[Route("api/acceso-temporal")]
/// <summary>
/// Expone las operaciones para generar y validar links de acceso temporal.
/// </summary>
public class AccesoTemporalController : ControllerBase
{
    private readonly IAccesoTemporalApplication AccesoTemporalAppProvider;

    /// <summary>
    /// Inicializa el controlador de acceso temporal.
    /// </summary>
    /// <param name="accesoTemporalApp">Servicio de aplicacion del mecanismo de acceso temporal.</param>
    public AccesoTemporalController(IAccesoTemporalApplication accesoTemporalApp)
    {
        AccesoTemporalAppProvider = accesoTemporalApp;
    }

    /// <summary>
    /// Genera un link temporal de un solo uso para un usuario externo.
    /// </summary>
    /// <param name="request">Datos minimos para asociar el token al expediente, actividad y usuario cliente.</param>
    /// <returns>URL completa que puede abrir el usuario externo.</returns>
    [Authorize]
    [HttpPost("generar")]
    public async Task<IActionResult> Generar([FromBody] AccesoTemporalGenerarRequestDTO request)
    {
        try
        {
            var result = await AccesoTemporalAppProvider.Generar(request, GetUserId());
            return Ok(new { status = true, detail = result, message = "Token temporal generado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
    }

    /// <summary>
    /// Valida un token publico y lo consume solo si puede emitir un JWT temporal.
    /// </summary>
    /// <param name="request">Token UUID recibido desde el link publico.</param>
    /// <returns>JWT temporal y ruta protegida de destino.</returns>
    [AllowAnonymous]
    [HttpPost("validar")]
    public async Task<IActionResult> Validar([FromBody] AccesoTemporalValidarRequestDTO request)
    {
        try
        {
            var result = await AccesoTemporalAppProvider.Validar(request);
            return Ok(new { status = true, detail = result, message = "Token temporal validado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = GetClientMessage(ex.Message) });
        }
    }

    /// <summary>
    /// Obtiene el identificador del usuario autenticado desde los claims del JWT.
    /// </summary>
    /// <returns>Identificador interno del usuario autenticado.</returns>
    private int GetUserId()
    {
        // La generacion queda auditada con el usuario autenticado que emitio el link.
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userIdClaim = identity?.Claims.FirstOrDefault(q => q.Type == "user_id");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }

    /// <summary>
    /// Traduce codigos internos de validacion a mensajes seguros para el cliente externo.
    /// </summary>
    /// <param name="code">Codigo funcional retornado por la validacion del token.</param>
    /// <returns>Mensaje amigable para mostrar en frontend o clientes API.</returns>
    private static string GetClientMessage(string code)
    {
        // Los codigos internos se traducen para no exponer detalles tecnicos al cliente externo.
        return code switch
        {
            Constants.AccesoTemporal.TokenExpirado =>
                "El enlace de acceso ha expirado. Comuníquese con su asesor para obtener un nuevo enlace.",
            Constants.AccesoTemporal.TokenUsado =>
                "El enlace de acceso ya fue utilizado. Comuniquese con su asesor BBVA para obtener un nuevo enlace.",
            Constants.AccesoTemporal.TokenInvalido =>
                "El enlace de acceso no es valido. Verifique el enlace o comuniquese con su asesor BBVA.",
            _ => "No fue posible validar el enlace de acceso temporal."
        };
    }
}
