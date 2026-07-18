using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.DTO.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Authorize(Roles = "ADMINISTRADOR")]
    [Route("api/[controller]")]
    [ApiController]
    public class BandejaActividadesController : ControllerBase
    {
        private readonly IBandejaActividadesApplication BandejaActividadesApplicationProvider;
        private readonly IActividadesApplication ActividadesApplicationProvider;

        public BandejaActividadesController(IBandejaActividadesApplication _bandejaActividadesApplication, IActividadesApplication _actividadesApplication)
        {
            BandejaActividadesApplicationProvider = _bandejaActividadesApplication;
            ActividadesApplicationProvider = _actividadesApplication;
        }

        [HttpGet, Route("getInfoActivityByUser")]
        public async Task<IActionResult> GetInfoActivityByUser()
        {
            IActionResult response = Unauthorized();

            List<ActividadDTO> listActivitiesDTO =
                await BandejaActividadesApplicationProvider.GetInfoActivityByUser(
                    GetUserId(),
                    Constants.WorkFlowMultibanca.WorkFlowID
                );

            response = Ok(new
            {
                status = true,
                detail = listActivitiesDTO ?? new List<ActividadDTO>(),
                message = "Información de actividades correctamente"
            });

            return response;
        }

        [HttpPost, Route("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] ActividadDTO actividadDTO)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (actividadDTO == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El objeto ActividadDTO es obligatorio."
                    });
                }
               var actividad = await ActividadesApplicationProvider.ObtenerActividadPorExpedienteActividad(actividadDTO.id_expediente, actividadDTO.id_actividad);
                if(actividad.status == "Nueva")
                {
                    actividad.status = "En progreso";
                }
                ActividadesApplicationProvider.Update(
                    actividad,
                    GetUserId()
                );

                response = Ok(new
                {
                    status = true,
                    detail = actividadDTO,
                    message = "Estado de actividad actualizado correctamente."
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