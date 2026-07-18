using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
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
    public class RecibirInstruccionPagoController : ControllerBase
    {
        private readonly IRecibirInstruccionPagoApplication RecibirInstruccionPagoApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.VisarOperacion;

        public RecibirInstruccionPagoController(
            IRecibirInstruccionPagoApplication _recibirInstruccionPagoApplication,
            ICommonApplication commonApplicationProvider,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RecibirInstruccionPagoApplicationProvider = _recibirInstruccionPagoApplication;
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RecibirInstruccionPagoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Recibir Instruccion Pago obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] recibir_instruccion_pago recibir_instruccion_pago)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (recibir_instruccion_pago.id_recibir_instruccion_pago == 0)
                {
                    recibir_instruccion_pago.row_status = true;
                    recibir_instruccion_pago.is_active = true;
                    recibir_instruccion_pago = RecibirInstruccionPagoApplicationProvider.Create(recibir_instruccion_pago, GetUserId());
                }
                else
                {
                    recibir_instruccion_pago = RecibirInstruccionPagoApplicationProvider.Update(recibir_instruccion_pago, GetUserId());
                }
                if (recibir_instruccion_pago == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(recibir_instruccion_pago.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = recibir_instruccion_pago.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = recibir_instruccion_pago.observaciones;
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
                    detail = recibir_instruccion_pago,
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

        [HttpGet, Route("getControlesRecibirInstruccionPago")]
        public async Task<IActionResult> GetControlesRecibirInstruccionPago()
        {
            try
            {
                var _condiciondesembolso = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.CondicionDesembolso);


                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        condicion_desembolso = _condiciondesembolso,

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

                List<AssignActivityDTO> listAssignActividadesDTO = await RecibirInstruccionPagoApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
