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
    public class RealizarRevisionPrevioFirmaBancoController : ControllerBase
    {
        private readonly IRealizarRevisionPrevioFirmaBancoApplication RealizarRevisionPrevioFirmaBancoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RealizarRevisionPrevioFirmaBanco;

        public RealizarRevisionPrevioFirmaBancoController(
            IRealizarRevisionPrevioFirmaBancoApplication _realizarRevisionPrevioFirmaBancoApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RealizarRevisionPrevioFirmaBancoApplicationProvider = _realizarRevisionPrevioFirmaBancoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RealizarRevisionPrevioFirmaBancoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Realizar Revision Previo Firma Banco obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] realizar_revision_previo_firma_banco realizar_revision_previo_firma_banco)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (realizar_revision_previo_firma_banco.id_realizar_revision_previo_firma_banco == 0)
                {
                    realizar_revision_previo_firma_banco.row_status = true;
                    realizar_revision_previo_firma_banco.is_active = true;
                    realizar_revision_previo_firma_banco = RealizarRevisionPrevioFirmaBancoApplicationProvider.Create(realizar_revision_previo_firma_banco, GetUserId());
                }
                else
                {
                    realizar_revision_previo_firma_banco = RealizarRevisionPrevioFirmaBancoApplicationProvider.Update(realizar_revision_previo_firma_banco, GetUserId());
                }
                if (realizar_revision_previo_firma_banco == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(realizar_revision_previo_firma_banco.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = realizar_revision_previo_firma_banco.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = realizar_revision_previo_firma_banco.observaciones;
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
                    detail = realizar_revision_previo_firma_banco,
                    message = "Realizar Revision Previo Firma Banco guardado correctamente."
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

        //[Authorize(Roles = "ADMINISTRADOR")]
        [AllowAnonymous]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await RealizarRevisionPrevioFirmaBancoApplicationProvider.Avanzar(
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
