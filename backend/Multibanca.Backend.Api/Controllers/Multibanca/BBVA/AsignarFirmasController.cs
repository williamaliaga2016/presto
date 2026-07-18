using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

[Authorize]
[ApiController]
[Route("api/asignar-firmas")]
public class AsignarFirmasController(IAsignarFirmasApplication application) : ControllerBase
{
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

    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] asignar_firmas_peritos_abogados request)
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

    [HttpPost("{idExpediente}/calcular-asignacion")]
    public async Task<IActionResult> Calcular(long idExpediente, [FromBody] CalcularAsignacionRequest request)
    {
        try
        {
            if (request.id_expediente != idExpediente)
                return Error("El expediente no coincide.");

            var result = await application.Calcular(request);
            return Success(result, "Asignación calculada.");
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            var result = await application.Avanzar(idExpediente, GetUserId());
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
