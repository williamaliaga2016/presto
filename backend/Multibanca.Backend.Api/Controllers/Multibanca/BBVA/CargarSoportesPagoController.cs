using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Backend.Api.Extensions;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

/// <summary>
/// Expone las operaciones de la actividad Cargar Soportes de Pago.
/// </summary>
[Authorize]
[ApiController]
[Route("api/cargar-soportes-pago")]
public class CargarSoportesPagoController : ControllerBase
{
    private readonly ICargarSoportesPagoApplication CargarSoportesPagoAppProvider;
    private readonly IAccesoTemporalApplication AccesoTemporalAppProvider;

    /// <summary>
    /// Inicializa el controller de Cargar Soportes de Pago.
    /// </summary>
    /// <param name="cargarSoportesPagoApp">Servicio de aplicacion de Cargar Soportes de Pago.</param>
    /// <param name="accesoTemporalApp">Servicio de aplicacion para consumir tokens temporales.</param>
    public CargarSoportesPagoController(
        ICargarSoportesPagoApplication cargarSoportesPagoApp,
        IAccesoTemporalApplication accesoTemporalApp)
    {
        CargarSoportesPagoAppProvider = cargarSoportesPagoApp;
        AccesoTemporalAppProvider = accesoTemporalApp;
    }

    /// <summary>
    /// Consulta el registro de confirmacion de soportes de pago guardado para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con el registro activo o `null`.</returns>
    [HttpGet("{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            cargar_soportes_pago? result = await CargarSoportesPagoAppProvider.GetByExpediente(idExpediente);
            return Ok(new { status = true, detail = result, message = "Registro consultado." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status = false,
                detail = Constants.ApiMessages.ErrorInterno,
                message = Constants.ApiMessages.ErrorInterno
            });
        }
    }

    /// <summary>
    /// Consulta informacion general del cliente para presentacion en la pantalla externa.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con informacion general de cliente y analista.</returns>
    [HttpGet("{idExpediente}/info-cliente")]
    public async Task<IActionResult> GetInfoCliente(long idExpediente)
    {
        try
        {
            CargarSoportesPagoInfoDTO result = await CargarSoportesPagoAppProvider.GetInfoCliente(idExpediente);
            return Ok(new { status = true, detail = result, message = "Informacion del cliente consultada." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status = false,
                detail = Constants.ApiMessages.ErrorInterno,
                message = Constants.ApiMessages.ErrorInterno
            });
        }
    }

    /// <summary>
    /// Guarda la confirmacion de soportes de pago sin avanzar el workflow.
    /// </summary>
    /// <param name="request">Datos de confirmacion y observaciones de Cargar Soportes de Pago.</param>
    /// <returns>Respuesta API con el registro creado o actualizado.</returns>
    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] cargar_soportes_pago request)
    {
        try
        {
            cargar_soportes_pago result = await CargarSoportesPagoAppProvider.Guardar(request, User.GetUserId());
            return Ok(new { status = true, detail = result, message = "Registro guardado." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status = false,
                detail = Constants.ApiMessages.ErrorInterno,
                message = Constants.ApiMessages.ErrorInterno
            });
        }
    }

    /// <summary>
    /// Avanza la actividad Cargar Soportes de Pago cuando la confirmacion requerida existe.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con el resultado de asignacion del workflow.</returns>
    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            Guid? temporalToken = User.GetTemporalAccessToken();

            List<AssignActivityDTO> result = await CargarSoportesPagoAppProvider.Avanzar(
                idExpediente,
                User.GetUserId(),
                Constants.ActividadesBBVA.SoportesPago);

            if (temporalToken.HasValue)
            {
                await AccesoTemporalAppProvider.ConsumirDespuesDeAccionExitosa(
                    temporalToken.Value,
                    idExpediente,
                    Constants.ActividadesBBVA.SoportesPago);
            }

            return Ok(new { status = true, detail = result, message = "Actividad avanzada." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { status = false, detail = ex.Message, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                status = false,
                detail = Constants.ApiMessages.ErrorInterno,
                message = Constants.ApiMessages.ErrorInterno
            });
        }
    }
}
