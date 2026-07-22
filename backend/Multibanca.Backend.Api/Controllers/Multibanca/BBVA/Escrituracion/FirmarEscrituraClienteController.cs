using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA.Escrituracion;

[Authorize]
[ApiController]
[Route("api/firmar-escritura-cliente")]
public class FirmarEscrituraClienteController : ControllerBase
{
    private readonly IFirmarEscrituraClienteApplication ApplicationProvider;
    private readonly string ActividadID = Constants.ActividadesBBVA.EscrituracionFirmarEscrituraCliente;

    public FirmarEscrituraClienteController(IFirmarEscrituraClienteApplication application)
    {
        ApplicationProvider = application;
    }

    [HttpGet("GetByIdExpediente/{id_expediente}")]
    public async Task<IActionResult> GetByIdExpediente(long id_expediente)
    {
        try
        {
            var result = await ApplicationProvider.GetByExpediente(id_expediente);
            return Ok(new { status = true, detail = result, message = "Firmar Escritura Cliente consultado correctamente." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("Save")]
    public IActionResult Save([FromBody] firmar_escritura_cliente_bbva model)
    {
        try
        {
            if (model.id_expediente <= 0)
            {
                return BadRequest(new { status = false, detail = (object?)null, message = "No existe un id_expediente válido." });
            }

            model.id_actividad = ActividadID;

            if (model.id == 0)
            {
                model.row_status = true;
                model.is_active = true;
                model = ApplicationProvider.Create(model, GetUserId());
            }
            else
            {
                // Forzar campos de auditoría para que Update no los ponga en false/infinity
                model.row_status = true;
                model.is_active = true;
                model = ApplicationProvider.Update(model, GetUserId());
            }

            return Ok(new { status = true, detail = model, message = "Firmar Escritura Cliente guardado correctamente." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    //corregi por id_expediente despues de avanzar
    [HttpPost("avanzar/{id_expediente}")]
    public async Task<IActionResult> Avanzar(long id_expediente)
    {
        try
        {
            var userId = GetUserId();
            var result = await ApplicationProvider.Avanzar(id_expediente, userId);

            if (result.Count > 0)
            {
                return Ok(new { status = true, detail = result, message = "Actividad avanzada correctamente." });
            }

            return Ok(new { status = false, detail = (object?)null, message = "No se pudo avanzar la actividad." });
        }
        catch (InvalidOperationException ex)
        {
            return HandleException(ex);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("/controles")]
    public async Task<IActionResult> GetControles(long id_expediente)
    {
        try
        {
            var result = await ApplicationProvider.GetControles(id_expediente);
            return Ok(new { status = true, detail = result, message = "Controles consultados." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private IActionResult HandleException(Exception ex)
    {
        if (ex.Message.StartsWith("Datos Obligatorios Faltantes:", StringComparison.OrdinalIgnoreCase))
        {
            var campos = ex.Message
                .Replace("Datos Obligatorios Faltantes:", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return BadRequest(new
            {
                status = false,
                detail = "Datos Obligatorios Faltantes",
                campos_faltantes = campos,
                message = "Datos Obligatorios Faltantes"
            });
        }

        return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
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
