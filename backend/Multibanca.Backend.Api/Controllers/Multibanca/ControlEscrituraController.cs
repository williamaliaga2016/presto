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
    public class ControlEscrituraController : ControllerBase
    {
        private readonly IControlEscrituraApplication ControlEscrituraApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.ControlEscritura;

        public ControlEscrituraController(
            IControlEscrituraApplication _controlEscrituraApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            ControlEscrituraApplicationProvider = _controlEscrituraApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            WorkflowApplicationProvider = _workflowApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            if (idExpediente <= 0)
                return BadRequest(new { status = false, message = "El id_expediente es obligatorio." });

            try
            {
                var result = await ControlEscrituraApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Control de Escritura obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] control_escritura control_escritura)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (control_escritura.id_usuario_solicitante == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El usuario solicitante es obligatorio."
                    });
                }

                if (string.IsNullOrWhiteSpace(control_escritura.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias."
                    });
                }

                if (control_escritura.id_control_escritura == 0)
                {
                    control_escritura.row_status = true;
                    control_escritura.is_active = true;
                    control_escritura = ControlEscrituraApplicationProvider.Create(control_escritura, GetUserId());
                }
                else
                {
                    control_escritura = ControlEscrituraApplicationProvider.Update(control_escritura, GetUserId());
                }

                if (control_escritura == null)
                    return BadRequest("Invalid client request");

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(control_escritura.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = control_escritura.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = control_escritura.observaciones;
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
                    detail = control_escritura,
                    message = "Control de Escritura guardado correctamente."
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

                List<AssignActivityDTO> listAssignActividadesDTO = await ControlEscrituraApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
            catch (InvalidOperationException ex)
            {
                response = BadRequest(new
                {
                    status = false,
                    message = ex.Message
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
