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
    public class GenerarCartaResguardoController : ControllerBase
    {
        private readonly IGenerarCartaResguardoApplication GenerarCartaResguardoApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.GenerarCartaResguardo;

        public GenerarCartaResguardoController(
            IGenerarCartaResguardoApplication _generarCartaResguardoApplication,
            ICommonApplication commonApplicationProvider,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            GenerarCartaResguardoApplicationProvider = _generarCartaResguardoApplication; 
            CommonApplicationProvider = commonApplicationProvider;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await GenerarCartaResguardoApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Generar carta resguardo obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_carta_resguardo generar_carta_resguardo)
        {
            IActionResult response = Unauthorized();

            try
            {

                if (generar_carta_resguardo.id_generar_carta_resguardo == 0)
                {
                    generar_carta_resguardo.row_status = true;
                    generar_carta_resguardo.is_active = true;
                    generar_carta_resguardo = GenerarCartaResguardoApplicationProvider.Create(generar_carta_resguardo, GetUserId());
                }
                else
                {
                    generar_carta_resguardo = GenerarCartaResguardoApplicationProvider.Update(generar_carta_resguardo, GetUserId());
                }
                if (generar_carta_resguardo == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(generar_carta_resguardo.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = generar_carta_resguardo.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = generar_carta_resguardo.observaciones;
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
                    detail = generar_carta_resguardo,
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
                    await GenerarCartaResguardoApplicationProvider.Avanzar(
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

        [HttpGet, Route("GetControlesGenerarCartaResguardo")]
        public async Task<IActionResult> GetControlesGenerarCartaResguardo()
        {
            try
            {
                var _tipoCarta = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.TipoCarta);
                
                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        tipocarta = _tipoCarta

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
