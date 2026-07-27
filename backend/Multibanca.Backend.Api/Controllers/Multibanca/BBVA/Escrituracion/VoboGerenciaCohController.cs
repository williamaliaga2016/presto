using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca.BBVA.Escrituracion;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA.Escrituracion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA.Escrituracion;

[Authorize(Roles = "ADMINISTRADOR,GERENTE_COH")]
[Route("api/[controller]")]
[ApiController]
public class VoboGerenciaCohController : ControllerBase
{
    private readonly IVoboGerenciaCohApplication _app;
    private readonly string ActividadID = Constants.ActividadesBBVA.EscrituracionVoBoGerenciaApplication;


    public VoboGerenciaCohController(IVoboGerenciaCohApplication app){
        _app = app;   
    }
    // GET /api/VoboGerenciaCoh/GetByExpediente/{idExpediente}
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

    // POST /api/VoboGerenciaCoh/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] vobo_gerencia_coh_bbva model)
    {
        try
        {
            if (model.id_expediente <= 0)
            {
                return BadRequest(new {status  = false,message = "El campo id_expediente es obligatorio."});
            }

            model.id_actividad = ActividadID;

            if (model.id == 0)
            {
                model.row_status = true;
                model.is_active = true;
                model = _app.Create(model, GetUserId());
            }
            else
            {
                // Forzar campos de auditoría para que Update no los ponga en false/infinity
                model.row_status = true;
                model.is_active = true;
                model = _app.Update(model, GetUserId());
            }

            return Ok(new { status = true, detail = model, message = "Firmar Escritura Cliente guardado correctamente." });

            // var result = await _app.Guardar(model, GetUserId());
            // return Ok(new
            // {
            //     status  = true,
            //     detail  = result,
            //     message = "Información guardada correctamente."
            // });
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

    // GET /api/VoboGerenciaCoh/Avanzar/{idExpediente}
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
                    detail  = new { actividad_destino = "Realizar Excepción Desembolso" },
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
            // Errores de validación de negocio (campos obligatorios, sin registro previo)
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
