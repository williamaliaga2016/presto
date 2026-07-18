using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Workflow;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisarLiquidacionController : ControllerBase
    {
        private readonly IRevisarLiquidacionApplication RevisarLiquidacionApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.RevisarLiquidacion;

        public RevisarLiquidacionController(
            IRevisarLiquidacionApplication _revisarLiquidacionApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RevisarLiquidacionApplicationProvider = _revisarLiquidacionApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            WorkflowApplicationProvider = _workflowApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarLiquidacionApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Revisar Liquidacion obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] revisar_liquidacion revisar_liquidacion)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (revisar_liquidacion.id_usuario_solicitante == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El usuario solicitante es obligatorio."
                    });
                }

                if (string.IsNullOrWhiteSpace(revisar_liquidacion.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias."
                    });
                }

                if (revisar_liquidacion.id_revisar_liquidacion == 0)
                {
                    revisar_liquidacion.row_status = true;
                    revisar_liquidacion.is_active = true;
                    revisar_liquidacion = RevisarLiquidacionApplicationProvider.Create(revisar_liquidacion, GetUserId());
                }
                else
                {
                    revisar_liquidacion = RevisarLiquidacionApplicationProvider.Update(revisar_liquidacion, GetUserId());
                }

                if (revisar_liquidacion == null)
                    return BadRequest("Invalid client request");

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(revisar_liquidacion.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_liquidacion.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = revisar_liquidacion.observaciones;
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
                    detail = revisar_liquidacion,
                    message = "Revisar Liquidacion guardado correctamente."
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
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await RevisarLiquidacionApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente."
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
