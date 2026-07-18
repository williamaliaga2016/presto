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
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisarInscripcionCbrController : ControllerBase
    {
        private readonly IRevisarInscripcionCbrApplication RevisarInscripcionCbrApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RecepcionCargaFabrica;

        public RevisarInscripcionCbrController(IRevisarInscripcionCbrApplication _revisarInscripcionCbrApplication, 
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication, 
            IActividadesApplication _actividadesApplication)
        {
            RevisarInscripcionCbrApplicationProvider = _revisarInscripcionCbrApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarInscripcionCbrApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Revisar Inscripcion CBR obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] revisar_inscripcion_cbr revisar_inscripcion_cbr)
        {
            IActionResult response = Unauthorized();
            
            try
            {
                revisar_inscripcion_cbr.id_usuario_solicitante = GetUserId();

                if (revisar_inscripcion_cbr.is_enviar_reparo && string.IsNullOrWhiteSpace(revisar_inscripcion_cbr.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se envía a reparo."
                    });
                }

                if (revisar_inscripcion_cbr.id_revisar_inscripcion_cbr == 0)
                {
                    revisar_inscripcion_cbr.row_status = true;
                    revisar_inscripcion_cbr.is_active = true;
                    revisar_inscripcion_cbr = RevisarInscripcionCbrApplicationProvider.Create(revisar_inscripcion_cbr, GetUserId());                    
                }
                else
                {
                    revisar_inscripcion_cbr = RevisarInscripcionCbrApplicationProvider.Update(revisar_inscripcion_cbr, GetUserId());
                }
                if (revisar_inscripcion_cbr == null)
                {
                    return BadRequest("Invalid client request");
                }
                else
                {
                    var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(revisar_inscripcion_cbr.id_expediente, ActividadID);

                    bitacora bitacora = new bitacora();
                    bitacora.id_expediente = revisar_inscripcion_cbr.id_expediente;
                    bitacora.id_actividad = ActividadID;
                    bitacora.id_usuario = GetUserId();
                    bitacora.fecha_alta = DateTime.Now;
                    bitacora.observaciones = revisar_inscripcion_cbr.observaciones;
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
                    detail = revisar_inscripcion_cbr,
                    message = "Revisar Inscripcion CBR guardada correctamente"
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


                List<AssignActivityDTO> listAssignActividadesDTO = await RevisarInscripcionCbrApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
