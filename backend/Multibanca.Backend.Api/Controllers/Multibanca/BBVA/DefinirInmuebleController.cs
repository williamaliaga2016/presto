using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

/// <summary>
/// Endpoints de la actividad BBV-44 Definir Inmueble.
/// </summary>
[Authorize]
[ApiController]
[Route("api/definir-inmueble")]
public class DefinirInmuebleController(IDefinirInmuebleApplication application) : ControllerBase
{
    /// <summary>
    /// Consulta el formulario con datos heredados y encabezado.
    /// </summary>
    [HttpGet("{idExpediente}/con-encabezado")]
    public async Task<IActionResult> GetConEncabezado(long idExpediente)
    {
        try
        {
            var result = await application.GetConEncabezado(idExpediente);
            return Success(result, "Formulario consultado.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    /// <summary>
    /// Consulta controles y catalogos de la pantalla.
    /// </summary>
    [HttpGet("{idExpediente}/controles")]
    public async Task<IActionResult> GetControles(long idExpediente)
    {
        try
        {
            var result = await application.GetControles();
            return Success(result, "Controles consultados.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    /// <summary>
    /// Guarda los datos propios de Definir Inmueble.
    /// </summary>
    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] definir_inmueble_bbva request)
    {
        try
        {
            var result = await application.Guardar(request, GetUserId());
            return Success(result, "Registro guardado.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    /// <summary>
    /// Avanza la actividad o solicita confirmacion cuando el inmueble sigue sin definirse.
    /// </summary>
    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(
        long idExpediente,
        [FromBody] DefinirInmuebleAvanzarRequestDTO? request)
    {
        try
        {
            var result = await application.Avanzar(
                idExpediente,
                GetUserId(),
                request?.confirmar ?? false);
            return Success(result, "Actividad avanzada.");
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
        return Error(exception.Message);
    }

    private IActionResult Error(string message)
    {
        return BadRequest(new { status = false, detail = message, message });
    }

    private int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userIdClaim = identity?.Claims.FirstOrDefault(q => q.Type == "user_id");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }
}
