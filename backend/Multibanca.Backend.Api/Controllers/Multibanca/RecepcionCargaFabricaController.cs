using Framework.WorkFlow.Application.Interfaces;
using Framework.WorkFlow.Common.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Backend.Api.Extensions;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecepcionCargaFabricaController : ControllerBase
    {
        private readonly IRecepcionCargaFabricaApplication RecepcionCargaFabricaApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RecepcionCargaFabrica;

        public RecepcionCargaFabricaController(IRecepcionCargaFabricaApplication _recepcionCargaFabricaApplication, 
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication, 
            IActividadesApplication _actividadesApplication)
        {
            RecepcionCargaFabricaApplicationProvider = _recepcionCargaFabricaApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RecepcionCargaFabricaApplicationProvider.GetByExpediente(idExpediente);

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
        public async Task<IActionResult> Save([FromBody] recepcion_carga_fabrica recepcion_carga_fabrica)
        {
            IActionResult response = Unauthorized();
            
            try
            {
                recepcion_carga_fabrica.id_usuario_solicitante = GetUserId();

                if (recepcion_carga_fabrica.is_enviar_reparo && string.IsNullOrWhiteSpace(recepcion_carga_fabrica.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se envía a reparo."
                    });
                }

                if (recepcion_carga_fabrica.id_recepcion_carga_fabrica == 0)
                {
                    recepcion_carga_fabrica.row_status = true;
                    recepcion_carga_fabrica.is_active = true;
                    recepcion_carga_fabrica = RecepcionCargaFabricaApplicationProvider.Create(recepcion_carga_fabrica, GetUserId());                    
                }
                else
                {
                    recepcion_carga_fabrica = RecepcionCargaFabricaApplicationProvider.Update(recepcion_carga_fabrica, GetUserId());
                }
                if (recepcion_carga_fabrica == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(recepcion_carga_fabrica.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = recepcion_carga_fabrica.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.observaciones = recepcion_carga_fabrica.observaciones;
                    bitacora.is_active = true;
                    bitacora.row_status = true;

                    if (resultBitacora.id_bitacora>0)
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
                    detail = recepcion_carga_fabrica,
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


                List<AssignActivityDTO> listAssignActividadesDTO = await RecepcionCargaFabricaApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count() > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente"
                    });
                    //bool resultNotificacion = await CommonApplicationProvider.NotificacionAvanceActividad(1, id_expediente, ActividadesID, id_usuario);
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
            return HttpContext.User.GetUserId();
        }
        #endregion

        #region GetPerformerId
        private string GetPerformerId()
        {
            return HttpContext.User.GetPerformerId();
        }
        #endregion
    }
}
