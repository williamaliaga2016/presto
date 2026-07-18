using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Application.Interfaces.Multibanca.DatosOperacion;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.Domain.Models.Multibanca.CargaOperacionBanco;
using Multibanca.Domain.Models.Multibanca.DatosOperacion;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisarIngresoDatosCreditoController : ControllerBase
    {
        private readonly IRevisarIngresoDatosCreditoApplication RevisarIngresoDatosCreditoProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RevisarIngresoDatosOperacion;

        public RevisarIngresoDatosCreditoController(
            IRevisarIngresoDatosCreditoApplication _revisarIngresoDatosCreditoApplication,
            IBitacoraApplication _bitacoraApplication,
            ICommonApplication _commonApplication)
        {
            RevisarIngresoDatosCreditoProvider = _revisarIngresoDatosCreditoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = _commonApplication;
        }

        // ============================================================
        // ACTIVIDAD BASE: DATOS DE OPERACIÓN - 5.11
        // ============================================================

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarIngresoDatosCreditoProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de Credito obtenidos correctamente."
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
        public async Task<IActionResult> Save([FromBody] revisar_ingreso_datos_credito revisar_ingreso_datos_credito)
        {
            IActionResult response = Unauthorized();

            try
            {

                int userId = GetUserId();


                // 2. Guardar tab Datos Crédito.
                if (revisar_ingreso_datos_credito.id_revisar_ingreso_datos_credito == 0)
                {
                    revisar_ingreso_datos_credito.is_active = true;
                    revisar_ingreso_datos_credito.row_status = true;
                    revisar_ingreso_datos_credito = RevisarIngresoDatosCreditoProvider.Create(revisar_ingreso_datos_credito, userId);
                }
                else
                {
                    revisar_ingreso_datos_credito = RevisarIngresoDatosCreditoProvider.Update(revisar_ingreso_datos_credito, userId);
                }


                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(revisar_ingreso_datos_credito.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = revisar_ingreso_datos_credito.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = userId;
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = "Se guardó la actividad Datos de Operación.";
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, userId);

                response = Ok(new
                {
                    status = true,
                    detail = revisar_ingreso_datos_credito,
                    message = "Datos de Operación guardados correctamente."
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

        // ============================================================
        // CONSULTAS POR TAB
        // ============================================================

        [HttpGet, Route("GetDatosCreditoByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetDatosCreditoByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarIngresoDatosCreditoProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos de Crédito obtenidos correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        //Region controles

        [HttpGet, Route("GetControlesDatosCredito")]
        public async Task<IActionResult> GetControlesDatosCredito()
        {
            try
            {
                var siNo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionSiNo);
                var tipoOperacion = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoOperacion);
                var tipoDireccionDividendo = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.DatosOperacionTipoDireccionDividendo);

                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        si_no = siNo,
                        tipo_operacion = tipoOperacion,
                        tipo_direccion_dividendo = tipoDireccionDividendo
                    },
                    message = "Controles de Datos Crédito obtenidos correctamente."
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
