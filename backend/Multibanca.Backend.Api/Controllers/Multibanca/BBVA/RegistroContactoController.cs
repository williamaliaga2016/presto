using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

/// <summary>
/// Endpoints del historico transversal de Registro Contacto BBVA.
/// </summary>
[Authorize]
[ApiController]
[Route("api/registro-contacto")]
public class RegistroContactoController(IRegistroContactoApplication application) : ControllerBase
{
    /// <summary>
    /// Consulta los catalogos propios de Registro Contacto.
    /// </summary>
    /// <returns>Respuesta API con catalogos de canal, resultado y detalle.</returns>
    [HttpGet("controles")]
    public async Task<IActionResult> GetControles()
    {
        try
        {
            object result = await application.GetControles();
            return Success(result, "Controles consultados.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    /// <summary>
    /// Consulta el historico global de contactos de un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente.</param>
    /// <returns>Respuesta API con contactos en orden descendente.</returns>
    [HttpGet("{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            List<registro_contacto_bbva> result =
                await application.GetByExpediente(idExpediente);
            return Success(result, "Contactos consultados.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    /// <summary>
    /// Crea un contacto en el historico transversal del expediente.
    /// </summary>
    /// <param name="request">Datos del contacto capturados por el analista.</param>
    /// <returns>Respuesta API con el contacto creado.</returns>
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] registro_contacto_bbva request)
    {
        try
        {
            registro_contacto_bbva result = await application.Crear(request, GetUserId());
            return Success(result, "Contacto registrado.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    private IActionResult Success(object? detail, string message)
    {
        return Ok(new { status = true, detail, message });
    }

    private IActionResult Error(Exception exception)
    {
        return BadRequest(new
        {
            status = false,
            detail = exception.Message,
            message = exception.Message
        });
    }

    private int GetUserId()
    {
        ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
        Claim? userIdClaim = identity?.Claims.FirstOrDefault(q => q.Type == "user_id");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }
}
