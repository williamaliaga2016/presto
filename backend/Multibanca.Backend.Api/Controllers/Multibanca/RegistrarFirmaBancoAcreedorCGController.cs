using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class RegistrarFirmaBancoAcreedorCGController : ControllerBase
    {
        private readonly IRegistrarFirmaBancoAcreedorCGApplication RegistrarFirmaBancoAcreedorCGApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID =
            Constants.Actividades.RegistrarFirmaBancoAcreedorCG;

        public RegistrarFirmaBancoAcreedorCGController(
            IRegistrarFirmaBancoAcreedorCGApplication _registrarFirmaBancoAcreedorCGApplication,
            IBitacoraApplication _bitacoraApplication
        )
        {
            RegistrarFirmaBancoAcreedorCGApplicationProvider =
                _registrarFirmaBancoAcreedorCGApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                var result =
                    await RegistrarFirmaBancoAcreedorCGApplicationProvider.GetByExpediente(
                        id_expediente
                    );

                return Ok(
                    new
                    {
                        status = true,
                        detail = result,
                        message = "Registrar Firma Banco Acreedor CG obtenido correctamente.",
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { status = false, message = ex.Message }
                );
            }
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save(
            [FromBody] firma_banco_acreedor_cg firma_banco_acreedor_cg
        )
        {
            IActionResult response = Unauthorized();

            try
            {
                if (firma_banco_acreedor_cg.id_firma_banco_acreedor_cg == 0)
                {
                    firma_banco_acreedor_cg.row_status = true;
                    firma_banco_acreedor_cg.is_active = true;
                    firma_banco_acreedor_cg =
                        RegistrarFirmaBancoAcreedorCGApplicationProvider.Create(
                            firma_banco_acreedor_cg,
                            GetUserId()
                        );
                }
                else
                {
                    firma_banco_acreedor_cg =
                        RegistrarFirmaBancoAcreedorCGApplicationProvider.Update(
                            firma_banco_acreedor_cg,
                            GetUserId()
                        );
                }

                if (firma_banco_acreedor_cg == null)
                    return BadRequest("Invalid client request");

                var resultBitacora =
                    await BitacoraApplicacionProvider.GetByExpedienteActividad(
                        firma_banco_acreedor_cg.id_expediente,
                        ActividadID
                    );

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = firma_banco_acreedor_cg.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = firma_banco_acreedor_cg.observaciones;
                bitacora.is_active = true;
                bitacora.row_status = true;

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                }
                else
                    BitacoraApplicacionProvider.Create(bitacora, GetUserId());

                response = Ok(
                    new
                    {
                        status = true,
                        detail = firma_banco_acreedor_cg,
                        message = "Registrar Firma Banco Acreedor CG guardado correctamente.",
                    }
                );
            }
            catch (Exception ex)
            {
                response = StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { status = false, message = ex.Message }
                );
            }

            return response;
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await RegistrarFirmaBancoAcreedorCGApplicationProvider.Avanzar(
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
