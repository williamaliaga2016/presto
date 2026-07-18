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
    public class GestionRectificatoriaEscrituraFirmadaPostventaController : ControllerBase
    {
        private readonly IGestionRectificatoriaEscrituraFirmadaPostventaApplication GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly string ActividadID = Constants.Actividades.GestionRectificatoriaEscrituraFirmadaPostventa;

        public GestionRectificatoriaEscrituraFirmadaPostventaController(IGestionRectificatoriaEscrituraFirmadaPostventaApplication _gestionRectificatoriaEscrituraFirmadaPostventa,
            IBitacoraApplication _bitacoraApplication, ICommonApplication _commonApplication,
            IActividadesApplication _actividadesApplication)
        {
            GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider = _gestionRectificatoriaEscrituraFirmadaPostventa;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = _commonApplication;
        }
        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider.GetByExpediente(idExpediente);

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
        public async Task<IActionResult> Save([FromBody] gestion_rectificatoria_escritura_firmada_postventa gestion_rectificatoria_escritura_firmada_postventa)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (gestion_rectificatoria_escritura_firmada_postventa.id_usuario_solicitante == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El usuario solicitante del reparo es obligatorio."
                    });
                }

                if (gestion_rectificatoria_escritura_firmada_postventa.subsanar && string.IsNullOrWhiteSpace(gestion_rectificatoria_escritura_firmada_postventa.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se subsana el reparo."
                    });
                }

                if (gestion_rectificatoria_escritura_firmada_postventa.id_gestion_rectificatoria_escritura_firmada_postventa == 0)
                {
                    gestion_rectificatoria_escritura_firmada_postventa.row_status = true;
                    gestion_rectificatoria_escritura_firmada_postventa.is_active = true;
                    gestion_rectificatoria_escritura_firmada_postventa = GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider.Create(gestion_rectificatoria_escritura_firmada_postventa, GetUserId());
                }
                else
                {
                    gestion_rectificatoria_escritura_firmada_postventa = GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider.Update(gestion_rectificatoria_escritura_firmada_postventa, GetUserId());
                }
                if (gestion_rectificatoria_escritura_firmada_postventa == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(gestion_rectificatoria_escritura_firmada_postventa.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = gestion_rectificatoria_escritura_firmada_postventa.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = gestion_rectificatoria_escritura_firmada_postventa.observaciones;
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
                    detail = gestion_rectificatoria_escritura_firmada_postventa,
                    message = "Corregir Gestion Rectificatoria Escritura Firmada Postventa guardado correctamente."
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


                List<AssignActivityDTO> listAssignActividadesDTO = await GestionRectificatoriaEscrituraFirmadaPostventaApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
        [HttpGet, Route("GetControles")]
        public async Task<IActionResult> GetControlesPropiedad()
        {
            try
            {
                var tipoReparo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.GestionRectificatoriaEscriturFirmadaPostventaTipoReparo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipo_reparo = tipoReparo
                    },
                    message = "Controles de Propiedad obtenidos correctamente."
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
