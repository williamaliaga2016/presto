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
    public class RectificatoriaLegalCartaResguardoController : ControllerBase
    {
        private readonly IRectificatoriaLegalCartaResguardoApplication RectificatoriaLegalCartaResguardoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RectificatoriaLegalCartaResguardo;

        public RectificatoriaLegalCartaResguardoController(
            IRectificatoriaLegalCartaResguardoApplication _rectificatoriaLegalCartaResguardoApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RectificatoriaLegalCartaResguardoApplicationProvider = _rectificatoriaLegalCartaResguardoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RectificatoriaLegalCartaResguardoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Rectificatoria Legal Carta de Resguardo obtenida correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR,ADMINISTRADOR_CARTA_RESGUARDO")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromBody] rectificatoria_legal_carta_resguardo rectificatoria_legal_carta_resguardo)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (string.IsNullOrWhiteSpace(rectificatoria_legal_carta_resguardo.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias."
                    });
                }

                if (rectificatoria_legal_carta_resguardo.id_rectificatoria_legal_carta_resguardo == 0)
                {
                    rectificatoria_legal_carta_resguardo.row_status = true;
                    rectificatoria_legal_carta_resguardo.is_active = true;
                    rectificatoria_legal_carta_resguardo = RectificatoriaLegalCartaResguardoApplicationProvider.Create(rectificatoria_legal_carta_resguardo, GetUserId());
                }
                else
                {
                    rectificatoria_legal_carta_resguardo = RectificatoriaLegalCartaResguardoApplicationProvider.Update(rectificatoria_legal_carta_resguardo, GetUserId());
                }

                if (rectificatoria_legal_carta_resguardo == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(rectificatoria_legal_carta_resguardo.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = rectificatoria_legal_carta_resguardo.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = rectificatoria_legal_carta_resguardo.observaciones;
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
                    detail = rectificatoria_legal_carta_resguardo,
                    message = "Rectificatoria Legal Carta de Resguardo guardada correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR,ADMINISTRADOR_CARTA_RESGUARDO")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await RectificatoriaLegalCartaResguardoApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
