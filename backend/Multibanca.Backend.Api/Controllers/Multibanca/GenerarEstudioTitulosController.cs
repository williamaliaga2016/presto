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
    public class GenerarEstudioTitulosController: ControllerBase
    {
        private readonly IGenerarEstudioTitulosApplication GenerarEstudioTitulosApplication;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.GenerarEstudioTitulos;

        public GenerarEstudioTitulosController(
            IBitacoraApplication bitacoraApplicacionProvider,
            IWorkflowApplication workflowApplicationProvider,
            IGenerarEstudioTitulosApplication generarEstudioTitulosApplication)
        {
            BitacoraApplicacionProvider = bitacoraApplicacionProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            GenerarEstudioTitulosApplication = generarEstudioTitulosApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GenerarEstudioTitulosApplication.GetByExpediente(idExpediente);
                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Estudio de Títulos obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_estudio_titulos generar_estudio_titulos)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (generar_estudio_titulos.id_generar_estudio_titulos == 0)
                {
                    generar_estudio_titulos = GenerarEstudioTitulosApplication.Create(generar_estudio_titulos, GetUserId());
                }
                else
                {
                    generar_estudio_titulos = GenerarEstudioTitulosApplication.Update(generar_estudio_titulos, GetUserId());
                }

                if (generar_estudio_titulos == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(generar_estudio_titulos.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = generar_estudio_titulos.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        bitacora.observaciones = "Se actualizó la actividad Generar Estudio de Títulos.";
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                    {
                        bitacora.observaciones = "Se guardó la actividad Generar Estudio de Títulos.";
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = generar_estudio_titulos,
                    message = "Estudio de Títulos guardado correctamente."
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
                List<AssignActivityDTO> listAssignActividadesDTO = await GenerarEstudioTitulosApplication.Avanzar(id_expediente, id_usuario, ActividadID);

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
