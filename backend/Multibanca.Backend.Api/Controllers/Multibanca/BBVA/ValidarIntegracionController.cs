using Azure;
using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Implementations.Multibanca.BBVA;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.DTO.Multibanca.BBVA;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidarIntegracionController : ControllerBase
    {
        private readonly IValidarIntegracionApplication ValidarIntegracionApplicationProvider;
        private readonly string ActividadID = Constants.ActividadesBBVA.ValidarIntegracion;

        public ValidarIntegracionController(
            IValidarIntegracionApplication _validarIntegracionApplication)
        {
            ValidarIntegracionApplicationProvider = _validarIntegracionApplication;
        }

        [HttpGet, Route("{idExpediente}/con-encabezado")]
        public async Task<IActionResult> GetByExpedienteConEncabezado(long idExpediente)
        {
            try
            {
                var result = await ValidarIntegracionApplicationProvider.GetByExpedienteConEncabezado(idExpediente);
                return Ok(new 
                { 
                    status = true, 
                    detail = result, 
                    message = "Datos de integración cargados."
                });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpGet, Route("{idExpediente}/controles")]
        public async Task<IActionResult> GetControles(long idExpediente)
        {
            try
            {
                var result = await ValidarIntegracionApplicationProvider.GetControles();

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Controles obtenidos correctamente."
                });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpPost, Route("guardar")]
        public async Task<IActionResult> Save([FromBody] GuardarValidarIntegracionDocumentosDTO request)
        {
            try
            {
                // Validamos que existan datos de validacion de documentos
                if (request.validar_informacion_data is null)
                {
                    return BadRequest(new { status = false, message = "No se encontraron datos de validacion de documentos." });
                }

                // Validacion rapida de controles
                if (request.formulario.documentos_correctos == false && string.IsNullOrWhiteSpace(request.formulario.motivo_devolucion))
                {
                    return BadRequest(new { status = false, message = "El motivo de devolución es obligatorio si rechaza los documentos." });
                }

                // Guardamos la actividad y recuperamos el Id
                GuardarValidarIntegracionDocumentosDTO responseGuardado = await ValidarIntegracionApplicationProvider.Guardar(request, GetUserId(), ActividadID);

                // Devolvemos la actividad actual
                var responseActividad = await ValidarIntegracionApplicationProvider.GetByExpedienteConEncabezado(responseGuardado.formulario.id_expediente);

                if (responseActividad == null)
                {
                    return BadRequest(new { status = false, message = "Invalid client request" });
                }

                return Ok(new { status = true, detail = responseActividad, message = "Guardado con éxito." });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpPost, Route("{id_expediente}/avanzar")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {

            IActionResult response = Unauthorized();

            try
            {
                var listAssignActividadesDTO = await ValidarIntegracionApplicationProvider.Avanzar(id_expediente, GetUserId(), ActividadID);

                response = Ok(new
                {
                    status = true,
                    detail = "",
                    message = "Actividad avanzada correctamente."
                });

                return response;
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        #region INTERVINIENTES

        [HttpGet("{idExpediente}/intervinientes")]
        public async Task<IActionResult> GetIntervinientes(long idExpediente)
        {
            try
            {
                var result = await ValidarIntegracionApplicationProvider.GetIntervinientes(idExpediente);
                return Ok(new { status = true, detail = result, message = "Intervinientes obtenidos correctamente."});
            }
            catch (BusinessException ex)
            {
                return Ok(new{ status = false,  message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode( StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpPost("{idExpediente}/interviniente")]
        public async Task<IActionResult> GuardarInterviniente(long idExpediente, [FromBody] IntervinienteDTO dto)
        {
            dto.id_expediente = idExpediente;

            var result = await ValidarIntegracionApplicationProvider.GuardarInterviniente(dto, GetUserId(), ActividadID);

            return Ok(result);
        }

        #endregion

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
