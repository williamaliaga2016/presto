using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA.Escrituracion;

[Authorize]
[ApiController]
[Route("api/revisar-ep-abogado")]
public class RevisarEpAbogadoController : ControllerBase
{
    private readonly IRevisarEpAbogadoApplication ApplicationProvider;
    private readonly string ActividadID = Constants.ActividadesBBVA.EscrituracionRevisarEPAbogado;

    public RevisarEpAbogadoController(IRevisarEpAbogadoApplication application)
    {
        ApplicationProvider = application;
    }

    [HttpGet("GetByIdExpediente/{id_expediente}")]
    public async Task<IActionResult> GetByIdExpediente(long id_expediente)
    {
        try
        {
            var result = await ApplicationProvider.GetByExpediente(id_expediente);
            return Ok(new { status = true, detail = result, message = "Revisar EP Abogado consultado correctamente." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("controles/{id_expediente}")]
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

    [HttpPost("Save")]
    public IActionResult Save([FromBody] revisar_ep_abogado_bbva model)
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
                model.row_status = true;
                model.is_active = true;
                model = ApplicationProvider.Update(model, GetUserId());
            }

            return Ok(new { status = true, detail = model, message = "Revisar EP Abogado guardado correctamente." });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

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
