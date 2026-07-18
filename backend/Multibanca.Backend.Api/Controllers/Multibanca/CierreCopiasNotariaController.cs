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
    public class CierreCopiasNotariaController : ControllerBase
    {
        private readonly ICierreCopiasNotariaApplication CierreCopiasNotariaApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RealizarCierreCopiasNotaria;

        public CierreCopiasNotariaController(
            ICierreCopiasNotariaApplication _cierreCopiasNotariaApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            CierreCopiasNotariaApplicationProvider = _cierreCopiasNotariaApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await CierreCopiasNotariaApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Cierre Copias Notaria obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] cierre_copias_notaria cierre_copias_notaria)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (cierre_copias_notaria.id_cierre_copias_notaria == 0)
                {
                    cierre_copias_notaria.row_status = true;
                    cierre_copias_notaria.is_active = true;
                    cierre_copias_notaria = CierreCopiasNotariaApplicationProvider.Create(cierre_copias_notaria, GetUserId());
                }
                else
                {
                    cierre_copias_notaria = CierreCopiasNotariaApplicationProvider.Update(cierre_copias_notaria, GetUserId());
                }
                if (cierre_copias_notaria == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(cierre_copias_notaria.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = cierre_copias_notaria.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = cierre_copias_notaria.observaciones;
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
                    detail = cierre_copias_notaria,
                    message = "Cierre Copias Notaria guardado correctamente."
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
                    await CierreCopiasNotariaApplicationProvider.Avanzar(
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
    }
}
