using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca;

[Authorize(Roles = "ADMINISTRADOR,ANALISTA_COBERTURA")]
[Route("api/[controller]")]
[ApiController]
public class RevisionMarcacionCoberturaController : ControllerBase
{
    private readonly IRevisionMarcacionCoberturaApplication _app;

    public RevisionMarcacionCoberturaController(IRevisionMarcacionCoberturaApplication app)
        => _app = app;

    // GET /api/RevisionMarcacionCobertura/GetByExpediente/{idExpediente}
    [HttpGet, Route("GetByExpediente/{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            var result = await _app.GetByExpediente(idExpediente);
            return Ok(new { status = true, detail = result, message = "OK" });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status  = false,
                message = "No fue posible obtener la información del expediente."
            });
        }
    }

    // GET /api/RevisionMarcacionCobertura/controles
    [HttpGet, Route("controles")]
    public async Task<IActionResult> GetControles()
    {
        try
        {
            var result = await _app.GetControles();
            return Ok(new { status = true, detail = result, message = "Controles consultados." });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status  = false,
                message = "No fue posible obtener los controles."
            });
        }
    }

    // POST /api/RevisionMarcacionCobertura/Save
    [HttpPost, Route("Save")]
    public async Task<IActionResult> Save([FromBody] revision_marcacion_cobertura_bbva model)
    {
        try
        {
            if (model.id_expediente <= 0)
                return Ok(new
                {
                    status  = false,
                    message = "El campo id_expediente es obligatorio."
                });

            var result = await _app.Guardar(model, GetUserId());
            return Ok(new
            {
                status  = true,
                detail  = result,
                message = "Información guardada correctamente."
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status  = false,
                message = "No fue posible guardar la información."
            });
        }
    }

    // GET /api/RevisionMarcacionCobertura/Avanzar/{idExpediente}
    [HttpGet, Route("Avanzar/{idExpediente}")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            var result = await _app.Avanzar(idExpediente, GetUserId());
            if (result.Count > 0)
                return Ok(new
                {
                    status  = true,
                    detail  = new { actividad_destino = "Validar Condiciones Desembolso" },
                    message = "Actividad avanzada correctamente."
                });

            return Ok(new
            {
                status  = false,
                message = "No fue posible completar la transición del flujo."
            });
        }
        catch (InvalidOperationException ex)
        {
            // Errores de validación de negocio (campos obligatorios, correo inválido, sin registro previo)
            return Ok(new { status = false, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status  = false,
                message = "No fue posible completar el avance."
            });
        }
    }

    #region Helpers
    private int GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userIdClaim = identity?.Claims.FirstOrDefault(c => c.Type == "user_id");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new InvalidOperationException("No fue posible identificar el usuario autenticado.");

        return userId;
    }
    #endregion
}
