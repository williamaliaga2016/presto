using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA.Escrituracion;

[Authorize(Roles = "ADMINISTRADOR,COMERCIAL")]
[ApiController]
[Route("api/excepcion-desembolso")]
public class ExcepcionDesembolsoController : ControllerBase
{
    private readonly IExcepcionDesembolsoApplication ApplicationProvider;
    private readonly string ActividadID = Constants.ActividadesBBVA.EscrituracionRealizarExcepcionDesembolso;

    public ExcepcionDesembolsoController(IExcepcionDesembolsoApplication application)
    {
        ApplicationProvider = application;
    }

    [HttpGet("GetByIdExpediente/{id_expediente}")]
    public async Task<IActionResult> GetByIdExpediente(long id_expediente)
    {
        try
        {
            if (id_expediente <= 0)
            {
                return BadRequest(new { status = false, detail = (object?)null, message = "No existe un id_expediente válido." });
            }

            var result = await ApplicationProvider.GetByExpediente(id_expediente);
            return Ok(new { status = true, detail = result, message = "Excepción Desembolso consultado correctamente." });
        }
        catch (Exception)
        {
            return BadRequest(new { status = false, detail = "Error interno", message = "No fue posible completar la operación." });
        }
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] excepcion_desembolso_bbva model)
    {
        try
        {
            if (model.id_expediente <= 0)
            {
                return BadRequest(new { status = false, detail = (object?)null, message = "No existe un id_expediente válido." });
            }

            model.id_actividad = ActividadID;
            var userId = GetUserId();
            var result = await ApplicationProvider.Guardar(model, userId);

            return Ok(new { status = true, detail = result, message = "Excepción Desembolso guardado correctamente." });
        }
        catch (Exception)
        {
            return BadRequest(new { status = false, detail = "Error interno", message = "No fue posible completar la operación." });
        }
    }

    [HttpPost("avanzar/{id_expediente}")]
    public async Task<IActionResult> Avanzar(long id_expediente)
    {
        try
        {
            if (id_expediente <= 0)
            {
                return BadRequest(new { status = false, detail = (object?)null, message = "No existe un id_expediente válido." });
            }

            var userId = GetUserId();
            var result = await ApplicationProvider.Avanzar(id_expediente, userId);

            // result vacío puede significar AND-JOIN (esperando ruta larga) — es exitoso
            return Ok(new { status = true, detail = result, message = result.Count > 0 ? "Actividad avanzada correctamente." : "Excepción completada. Esperando finalización de ruta larga para crear Validar Condiciones." });
        }
        catch (InvalidOperationException ex)
        {
            return HandleException(ex);
        }
        catch (Exception)
        {
            return BadRequest(new { status = false, detail = "Error interno", message = "No fue posible completar la operación." });
        }
    }

    private IActionResult HandleException(Exception ex)
    {
        if (ex.Message.StartsWith("Campos obligatorios faltantes:", StringComparison.OrdinalIgnoreCase))
        {
            var campos = ex.Message
                .Replace("Campos obligatorios faltantes:", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return BadRequest(new
            {
                status = false,
                detail = "Datos Obligatorios Faltantes",
                campos_faltantes = campos,
                message = "Datos Obligatorios Faltantes"
            });
        }

        if (ex.Message.Contains("Debe guardar la información antes de avanzar", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(new { status = false, detail = (object?)null, message = "Debe guardar la información antes de avanzar." });
        }

        if (ex.Message.Contains("No se encontró la transición", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(new { status = false, detail = (object?)null, message = "No fue posible completar la transición del flujo." });
        }

        return BadRequest(new { status = false, detail = "Error interno", message = "No fue posible completar la operación." });
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
