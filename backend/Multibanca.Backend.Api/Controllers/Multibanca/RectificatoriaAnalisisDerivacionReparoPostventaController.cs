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
    public class RectificatoriaAnalisisDerivacionReparoPostventaController : ControllerBase
    {
        private readonly IRectificatoriaAnalisisDerivacionReparoPostventaApplication RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RectificatoriaAnalisisDerivacionReparoPostventa;

        public RectificatoriaAnalisisDerivacionReparoPostventaController(
            IRectificatoriaAnalisisDerivacionReparoPostventaApplication _vectificatoriaAnalisisDerivacionReparoPostventaApplication,
            ICommonApplication commonApplicationProvider,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider = _vectificatoriaAnalisisDerivacionReparoPostventaApplication;
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.GetByExpediente(idExpediente);

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
        public async Task<IActionResult> Save([FromBody] rectificatoria_analisis_derivacion_reparo_postventa rectificatoria_analisis_derivacion_reparo_postventa)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (rectificatoria_analisis_derivacion_reparo_postventa.id_rectificatoria_analisis_derivacion_reparo_postventa == 0)
                {
                    rectificatoria_analisis_derivacion_reparo_postventa.row_status = true;
                    rectificatoria_analisis_derivacion_reparo_postventa.is_active = true;
                    rectificatoria_analisis_derivacion_reparo_postventa = RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.Create(rectificatoria_analisis_derivacion_reparo_postventa, GetUserId());
                }
                else
                {
                    rectificatoria_analisis_derivacion_reparo_postventa = RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.Update(rectificatoria_analisis_derivacion_reparo_postventa, GetUserId());
                }
                if (rectificatoria_analisis_derivacion_reparo_postventa == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(rectificatoria_analisis_derivacion_reparo_postventa.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = rectificatoria_analisis_derivacion_reparo_postventa.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = rectificatoria_analisis_derivacion_reparo_postventa.observaciones;
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
                    detail = rectificatoria_analisis_derivacion_reparo_postventa,
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
        [HttpGet, Route("getControlesRectificatoriaAnalisisDerivacionReparoPostventa")]
        public async Task<IActionResult> getControlesRectificatoriaAnalisisDerivacionReparoPostventa()
        {
            try
            {
                var _tiporeparo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoReparoRectificatoriaPostVenta);


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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await RectificatoriaAnalisisDerivacionReparoPostventaApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
