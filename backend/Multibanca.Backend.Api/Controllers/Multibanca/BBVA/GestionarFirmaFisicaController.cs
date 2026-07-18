using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Domain.Models.Multibanca.BBVA;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

[Authorize]
[ApiController]
[Route("api/gestionar-firma-fisica")]
public class GestionarFirmaFisicaController(IGestionarFirmaFisicaApplication application) : ControllerBase
{
    [HttpGet("{idExpediente}/con-encabezado")]
    public async Task<IActionResult> GetConEncabezado(long idExpediente)
    {
        try
        {
            var result = await application.GetFormularioConEncabezado(idExpediente);
            return Ok(new { status = true, detail = result, message = "Formulario consultado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
    }

    [HttpGet("{idExpediente}/controles")]
    public async Task<IActionResult> GetControles(long idExpediente)
    {
        try
        {
            var result = await application.GetControles(idExpediente);
            return Ok(new { status = true, detail = result, message = "Controles consultados." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
    }

    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] gestionar_firma_fisica_bbva request)
    {
        try
        {
            var userId = GetUserId();
            var result = await application.Guardar(request, userId);
            return Ok(new { status = true, detail = result, message = "Registro guardado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
    }

    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            var userId = GetUserId();
            var result = await application.Avanzar(idExpediente, userId);
            return Ok(new { status = true, detail = result, message = "Actividad avanzada." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
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
