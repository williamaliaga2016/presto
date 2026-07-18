using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
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
    public class VerificarReparoCbrController : ControllerBase
    {
        private readonly IVerificarReparoCbrApplication VerificarReparoCbrApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.VisarOperacion;

        public VerificarReparoCbrController(
            IVerificarReparoCbrApplication _verificarReparoCbrApplication,
            ICommonApplication commonApplicationProvider,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            VerificarReparoCbrApplicationProvider = _verificarReparoCbrApplication;
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await VerificarReparoCbrApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Verificar reparo CBR obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] verificar_reparo_cbr verificar_reparo_cbr)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (verificar_reparo_cbr.id_verificar_reparo_cbr == 0)
                {
                    verificar_reparo_cbr.row_status = true;
                    verificar_reparo_cbr.is_active = true;
                    verificar_reparo_cbr = VerificarReparoCbrApplicationProvider.Create(verificar_reparo_cbr, GetUserId());
                }
                else
                {
                    verificar_reparo_cbr = VerificarReparoCbrApplicationProvider.Update(verificar_reparo_cbr, GetUserId());
                }
                if (verificar_reparo_cbr == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(verificar_reparo_cbr.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = verificar_reparo_cbr.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = verificar_reparo_cbr.observaciones;
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
                    detail = verificar_reparo_cbr,
                    message = "Visar operación guardado correctamente."
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
        [HttpGet, Route("getControlesVerificarReparoCbr")]
        public async Task<IActionResult> GetControlesVerificarReparoCbr()
        {
            try
            {
                var _tiporeparo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoReparoVerificarCbr);


                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tiporeparo = _tiporeparo,

                    },
                    message = "Controles de Generar Borrador Escritura obtenidos correctamente."
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

        //[Authorize(Roles = "ADMINISTRADOR")]
        [AllowAnonymous]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                //int id_usuario = GetUserId();
                int id_usuario = 1;

                List<AssignActivityDTO> listAssignActividadesDTO = await VerificarReparoCbrApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
