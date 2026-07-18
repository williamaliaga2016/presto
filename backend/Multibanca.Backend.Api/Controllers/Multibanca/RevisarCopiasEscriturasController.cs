using Multibanca.Application.Interfaces.Workflow;
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
    public class RevisarCopiasEscriturasController : ControllerBase
    {
        private readonly IRevisarCopiasEscriturasApplication RevisarCopiasEscriturasApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RevisarCopiasEscrituras;

        public RevisarCopiasEscriturasController(
            IRevisarCopiasEscriturasApplication _revisarCopiasEscriturasApplication,
            ICommonApplication commonApplicationProvider,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RevisarCopiasEscriturasApplicationProvider = _revisarCopiasEscriturasApplication;
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarCopiasEscriturasApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Revisar Copias de Escrituras obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] revisar_copias_escrituras revisar_copias_escrituras)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (revisar_copias_escrituras.id_revisar_copias_escrituras == 0)
                {
                    revisar_copias_escrituras.row_status = true;
                    revisar_copias_escrituras.is_active = true;
                    revisar_copias_escrituras = RevisarCopiasEscriturasApplicationProvider.Create(revisar_copias_escrituras, GetUserId());
                }
                else
                {
                    revisar_copias_escrituras = RevisarCopiasEscriturasApplicationProvider.Update(revisar_copias_escrituras, GetUserId());
                }
                if (revisar_copias_escrituras == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(revisar_copias_escrituras.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_copias_escrituras.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = revisar_copias_escrituras.observaciones;
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
                    detail = revisar_copias_escrituras,
                    message = "Generar Carta Resguardo guardado correctamente."
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

        //[Authorize(Roles = "ADMINISTRADOR")]
        [AllowAnonymous]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await RevisarCopiasEscriturasApplicationProvider.Avanzar(
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

        [HttpGet, Route("GetControlesRevisarCopiasEscrituras")]
        public async Task<IActionResult> GetControlesRevisarCopiasEscrituras()
        {
            try
            {
                var _comuna = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionComuna);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        comuna = _comuna

                    },
                    message = "Controles de Generar Carta Resguardo obtenidos correctamente."
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
    }
}
