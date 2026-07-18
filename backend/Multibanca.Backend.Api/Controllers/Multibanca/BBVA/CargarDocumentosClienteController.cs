using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Backend.Api.Extensions;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca.BBVA;
using Multibanca.DTO.Multibanca.BBVA;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA;

/// <summary>
/// Expone las operaciones de la actividad Cargar Documentos Cliente.
/// </summary>
[Authorize]
[ApiController]
[Route("api/cargar-documentos-cliente")]
public class CargarDocumentosClienteController : ControllerBase
{
    private readonly ICargarDocumentosClienteApplication CargarDocumentosClienteAppProvider;
    private readonly IAccesoTemporalApplication AccesoTemporalAppProvider;

    /// <summary>
    /// Inicializa el controller de Cargar Documentos Cliente.
    /// </summary>
    /// <param name="cargarDocumentosClienteApp">Servicio de aplicacion de Cargar Documentos Cliente.</param>
    /// <param name="accesoTemporalApp">Servicio de aplicacion para consumir tokens temporales.</param>
    public CargarDocumentosClienteController(
        ICargarDocumentosClienteApplication cargarDocumentosClienteApp,
        IAccesoTemporalApplication accesoTemporalApp)
    {
        CargarDocumentosClienteAppProvider = cargarDocumentosClienteApp;
        AccesoTemporalAppProvider = accesoTemporalApp;
    }

    /// <summary>
    /// Consulta el registro de confirmacion documental guardado para un expediente.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con el registro activo o `null`.</returns>
    [HttpGet("{idExpediente}")]
    public async Task<IActionResult> GetByExpediente(long idExpediente)
    {
        try
        {
            cargar_documentos_cliente? result = await CargarDocumentosClienteAppProvider.GetByExpediente(idExpediente);
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
    /// Consulta informacion general del cliente para presentacion enmascarada en la pantalla externa.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con informacion general de cliente y analista.</returns>
    [HttpGet("{idExpediente}/info-cliente")]
    public async Task<IActionResult> GetInfoCliente(long idExpediente)
    {
        try
        {
            CargarDocumentosClienteInfoDTO result = await CargarDocumentosClienteAppProvider.GetInfoCliente(idExpediente);
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
    /// Guarda la confirmacion documental sin avanzar el workflow.
    /// </summary>
    /// <param name="request">Datos de confirmacion y observaciones de Cargar Documentos Cliente.</param>
    /// <returns>Respuesta API con el registro creado o actualizado.</returns>
    [HttpPost("guardar")]
    public async Task<IActionResult> Guardar([FromBody] cargar_documentos_cliente request)
    {
        try
        {
            cargar_documentos_cliente result = await CargarDocumentosClienteAppProvider.Guardar(request, User.GetUserId());
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
    /// Avanza la actividad Cargar Documentos Cliente hacia revision documental cuando la confirmacion requerida existe.
    /// </summary>
    /// <param name="idExpediente">Identificador del expediente en Presto.</param>
    /// <returns>Respuesta API con el resultado de asignacion del workflow.</returns>
    [HttpPost("{idExpediente}/avanzar")]
    public async Task<IActionResult> Avanzar(long idExpediente)
    {
        try
        {
            Guid? temporalToken = User.GetTemporalAccessToken();

            List<AssignActivityDTO> result = await CargarDocumentosClienteAppProvider.Avanzar(
                idExpediente,
                User.GetUserId(),
                Constants.ActividadesBBVA.DocsCliente);

            if (temporalToken.HasValue)
            {
                await AccesoTemporalAppProvider.ConsumirDespuesDeAccionExitosa(
                    temporalToken.Value,
                    idExpediente,
                    Constants.ActividadesBBVA.DocsCliente);
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
