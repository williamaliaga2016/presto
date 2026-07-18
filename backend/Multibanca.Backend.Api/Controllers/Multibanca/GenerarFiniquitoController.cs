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
    public class GenerarFiniquitoController : ControllerBase
    {
        private readonly IGenerarFiniquitoApplication GenerarFiniquitoApplication;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly IWorkflowApplication WorkflowApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.GenerarFiniquito;
        public GenerarFiniquitoController(IBitacoraApplication bitacoraApplicacionProvider, IWorkflowApplication workflowApplicationProvider, IGenerarFiniquitoApplication generarFiniquitoApplication)
        {
            BitacoraApplicacionProvider = bitacoraApplicacionProvider;
            WorkflowApplicationProvider = workflowApplicationProvider;
            GenerarFiniquitoApplication = generarFiniquitoApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GenerarFiniquitoApplication.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Finiquito obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_finiquito generar_finiquito)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (generar_finiquito.id_generar_finiquito == 0)
                {
                    generar_finiquito = GenerarFiniquitoApplication.Create(generar_finiquito, GetUserId());
                }
                else
                {
                    generar_finiquito = GenerarFiniquitoApplication.Update(generar_finiquito, GetUserId());
                }

                if (generar_finiquito == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(generar_finiquito.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = generar_finiquito.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        bitacora.observaciones = "Se actualizó la actividad Generar Finiquito.";
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                    {
                        bitacora.observaciones = "Se guardó la actividad Generar Finiquito.";
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                    }
                }

                response = Ok(new
                {
                    status = true,
                    detail = generar_finiquito,
                    message = "Finiquito guardado correctamente."
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
                List<AssignActivityDTO> listAssignActividadesDTO = await GenerarFiniquitoApplication.Avanzar(id_expediente, id_usuario, ActividadID);

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
