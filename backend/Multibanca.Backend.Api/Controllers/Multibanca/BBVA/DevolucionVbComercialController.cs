using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca.BBVA;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca.BBVA.Multibanca.Application.DTOs;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca.BBVA
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionVbComercialController : ControllerBase
    {
        private readonly IDevolucionVbComercialApplication DevolucionApplicationProvider;
        private readonly string ActividadID = Constants.ActividadesBBVA.DevolucionVbComercial;

        public DevolucionVbComercialController(IDevolucionVbComercialApplication _devolucionApplication)
        {
            DevolucionApplicationProvider = _devolucionApplication;
        }

        [HttpGet, Route("{idExpediente}/con-encabezado")]
        public async Task<IActionResult> GetByExpedienteConEncabezado(long idExpediente)
        {
            try
            {
                var result = await DevolucionApplicationProvider.GetByExpedienteConEncabezado(idExpediente);
                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de devolución comercial cargados."
                });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpGet, Route("{idExpediente}/controles")]
        public async Task<IActionResult> GetControles(long idExpediente)
        {
            try
            {
                var result = await DevolucionApplicationProvider.GetControles();
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR")] // Ajusta los roles autorizados según tu rama
        [HttpPost, Route("guardar")]
        public async Task<IActionResult> Save([FromBody] GuardarDevolucionVbComercialDTO request)
        {
            try
            {
                // Validacion rapida de controles
                if (request.formulario.cliente_desiste == true && string.IsNullOrWhiteSpace(request.formulario.motivo_cierre))
                {
                    return BadRequest(new { status = false, message = "El motivo de cierre es obligatorio si el cliente desiste." });
                }

                // Guardamos la actividad y recuperamos el Id
                GuardarDevolucionVbComercialDTO responseGuardado = await DevolucionApplicationProvider.Guardar(request, GetUserId(), ActividadID);

                // Devolvemos la actividad actual
                var responseActividad = await DevolucionApplicationProvider.GetByExpedienteConEncabezado(responseGuardado.formulario.id_expediente);


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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost, Route("{id_expediente}/avanzar")]
        public async Task<IActionResult> Avanzar(long id_expediente, [FromQuery] bool confirmar_cierre = false)
        {
            try
            {
                var listAssignActividadesDTO = await DevolucionApplicationProvider.Avanzar(id_expediente, GetUserId(), ActividadID, confirmar_cierre);

                return Ok(new
                {
                    status = true,
                    detail = "",
                    message = "Actividad avanzada correctamente."
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

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
