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
    public class CorregirReparosGestorController : ControllerBase
    {
        private readonly ICorregirReparosGestorApplication CorregirReparosGestorApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.CorregirReparosGestor;

        public CorregirReparosGestorController(
            ICorregirReparosGestorApplication _corregirReparosGestorApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            CorregirReparosGestorApplicationProvider = _corregirReparosGestorApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await CorregirReparosGestorApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparos por Gestor obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] corregir_reparos_gestor corregir_reparos_gestor)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (string.IsNullOrWhiteSpace(corregir_reparos_gestor.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias."
                    });
                }

                if (corregir_reparos_gestor.id_corregir_reparos_gestor == 0)
                {
                    corregir_reparos_gestor.row_status = true;
                    corregir_reparos_gestor.is_active = true;
                    corregir_reparos_gestor = CorregirReparosGestorApplicationProvider.Create(corregir_reparos_gestor, GetUserId());
                }
                else
                {
                    corregir_reparos_gestor = CorregirReparosGestorApplicationProvider.Update(corregir_reparos_gestor, GetUserId());
                }

                if (corregir_reparos_gestor == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Invalid client request"
                    });
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(corregir_reparos_gestor.id_expediente, ActividadID);

                bitacora bitacora = new bitacora
                {
                    id_expediente = corregir_reparos_gestor.id_expediente,
                    id_actividad = ActividadID,
                    id_usuario = GetUserId(),
                    fecha_alta = DateTime.Now,
                    observaciones = corregir_reparos_gestor.observaciones ?? string.Empty,
                    is_active = true,
                    row_status = true
                };

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                }
                else
                {
                    BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                }

                corregir_reparos_gestor? result =
                    await CorregirReparosGestorApplicationProvider.GetByExpediente(
                        corregir_reparos_gestor.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparos por Gestor guardado correctamente."
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

                List<AssignActivityDTO> listAssignActividadesDTO = await CorregirReparosGestorApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
