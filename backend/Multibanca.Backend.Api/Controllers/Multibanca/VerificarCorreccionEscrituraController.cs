using Framework.WorkFlow.Application.Interfaces;
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
    public class VerificarCorreccionEscrituraController : ControllerBase
    {
        private readonly IVerificarCorreccionEscrituraApplication VerificarCorreccionEscrituraApplication;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.VerificarCorreccionEscritura;
        public VerificarCorreccionEscrituraController(IBitacoraApplication bitacoraApplicacionProvider, IWorkflowApplication workflowApplicationProvider, IVerificarCorreccionEscrituraApplication verificarCorreccionEscrituraApplication) { 
            BitacoraApplicacionProvider = bitacoraApplicacionProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            VerificarCorreccionEscrituraApplication = verificarCorreccionEscrituraApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await VerificarCorreccionEscrituraApplication.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Verificar Correccion Escritura obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] verificar_correccion_escritura verificar_correccion_escritura)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (verificar_correccion_escritura.id_verificar_correccion_escritura == 0)
                {
                    verificar_correccion_escritura = VerificarCorreccionEscrituraApplication.Create(verificar_correccion_escritura, GetUserId());
                }
                else
                {
                    verificar_correccion_escritura = VerificarCorreccionEscrituraApplication.Update(verificar_correccion_escritura, GetUserId());
                }

                if (verificar_correccion_escritura == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(verificar_correccion_escritura.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = verificar_correccion_escritura.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        bitacora.observaciones = "Se actualizó la actividad Verificar Correccion Escritura.";
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                    {
                        bitacora.observaciones = "Se guardó la actividad Verificar Correccion Escritura.";
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = verificar_correccion_escritura,
                    message = "Verificar Correccion Escritura guardado correctamente."
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

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await VerificarCorreccionEscrituraApplication.Avanzar(
                        id_expediente,
                        id_usuario,
                        ActividadID
                    );

                response = Ok(new
                {
                    status = true,
                    detail = listAssignActividadesDTO,
                    message = "Actividad avanzada correctamente"
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

        #region GetPerformerId
        private string GetPerformerId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return claims[5].Value;
        }
        #endregion
    }
}
