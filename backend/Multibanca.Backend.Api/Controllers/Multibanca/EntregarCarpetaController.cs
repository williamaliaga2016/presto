using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregarCarpetaController : ControllerBase
    {
        private readonly IEntregarCarpetaApplication EntregarCarpetaApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.EntregarCarpeta;

        public EntregarCarpetaController(
            IEntregarCarpetaApplication _entregarCarpetaApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            EntregarCarpetaApplicationProvider = _entregarCarpetaApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await EntregarCarpetaApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Entregar carpeta obtenido correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromBody] entregar_carpeta entregar_carpeta)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (entregar_carpeta.id_entregar_carpeta == 0)
                {
                    entregar_carpeta.row_status = true;
                    entregar_carpeta.is_active = true;
                    entregar_carpeta = EntregarCarpetaApplicationProvider.Create(entregar_carpeta, GetUserId());
                }
                else
                {
                    entregar_carpeta = EntregarCarpetaApplicationProvider.Update(entregar_carpeta, GetUserId());
                }
                if (entregar_carpeta == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(entregar_carpeta.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = entregar_carpeta.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = entregar_carpeta.observaciones;
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, GetUserId());



                response = Ok(new
                {
                    status = true,
                    detail = entregar_carpeta,
                    message = "Entregar carpeta guardado correctamente."
                });
            }
            catch (Exception ex)
            {
                response = StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }

            return response;
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [AllowAnonymous]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await EntregarCarpetaApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente"
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                response = StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    message = ex.Message
                });
            }

            return response;
        }

        #region GetUserId
        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
        #endregion
    }
}
