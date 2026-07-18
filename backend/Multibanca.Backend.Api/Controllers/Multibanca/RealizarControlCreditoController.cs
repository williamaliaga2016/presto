using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealizarControlCreditoController : ControllerBase
    {
        private readonly IRealizarControlCreditoApplication RealizarControlCreditoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RealizarControlCredito;

        public RealizarControlCreditoController(IRealizarControlCreditoApplication _recepcionCargaFabricaApplication,
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication,
            IActividadesApplication _actividadesApplication)
        {
            RealizarControlCreditoApplicationProvider = _recepcionCargaFabricaApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RealizarControlCreditoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Recepción Carga Fábrica obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] realizar_control_credito realizar_control_credito)
        {
            IActionResult response = Unauthorized();

            try
            {
                realizar_control_credito.id_usuario_solicitante = GetUserId();

                if (realizar_control_credito.enviar_reparo && string.IsNullOrWhiteSpace(realizar_control_credito.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se envía a reparo."
                    });
                }

                if (realizar_control_credito.id_realizar_control_credito == 0)
                {
                    realizar_control_credito.row_status = true;
                    realizar_control_credito.is_active = true;
                    realizar_control_credito = RealizarControlCreditoApplicationProvider.Create(realizar_control_credito, GetUserId());
                }
                else
                {
                    realizar_control_credito = RealizarControlCreditoApplicationProvider.Update(realizar_control_credito, GetUserId());
                }
                if (realizar_control_credito == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(realizar_control_credito.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = realizar_control_credito.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.observaciones = realizar_control_credito.observaciones;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora > 0)
                    {
                        bitacora.id_bitacora = resultBitacora.id_bitacora;
                        BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                    }
                    else
                        BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                }

                response = Ok(new
                {
                    status = true,
                    detail = realizar_control_credito,
                    message = "Alta Solicitud guardada correctamente"
                });
                //bool isSend = await SendNotifications(13790);******************************************Prueba
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

        //Metodo Avanzar                
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await RealizarControlCreditoApplicationProvider.Avanzar(
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
