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
    public class GestionReparoController : ControllerBase
    {
        private readonly IGestionReparoApplication GestionReparoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.GestionReparo;

        public GestionReparoController(IGestionReparoApplication _gestionRectificatoriaSolucionReparo,
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication,
            IActividadesApplication _actividadesApplication)
        {
            GestionReparoApplicationProvider = _gestionRectificatoriaSolucionReparo;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GestionReparoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Recepción  obtenida correctamente."
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
        public async Task<IActionResult> Save([FromBody] gestion_reparo gestion_reparo)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (gestion_reparo.id_usuario_solicitante == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El usuario solicitante del reparo es obligatorio."
                    });
                }

                if (gestion_reparo.subsanar && string.IsNullOrWhiteSpace(gestion_reparo.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se subsana el reparo."
                    });
                }

                if (gestion_reparo.id_gestion_reparo == 0)
                {
                    gestion_reparo.row_status = true;
                    gestion_reparo.is_active = true;
                    gestion_reparo = GestionReparoApplicationProvider.Create(gestion_reparo, GetUserId());
                }
                else
                {
                    gestion_reparo = GestionReparoApplicationProvider.Update(gestion_reparo, GetUserId());
                }
                if (gestion_reparo == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(gestion_reparo.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = gestion_reparo.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = gestion_reparo.observaciones;
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
                    detail = gestion_reparo,
                    message = "Corregir Gestion del Reparo guardado correctamente."
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

        //Metodo Avanzar                
        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();
            try
            {
                int id_usuario = GetUserId();


                List<AssignActivityDTO> listAssignActividadesDTO = await GestionReparoApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
