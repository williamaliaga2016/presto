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
    public class RegistrarFechaRegistroCbrController : ControllerBase
    {
        private readonly IRegistrarFechaRegistroCbrApplication RegistrarFechaRegistroCbrApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RegistrarFechaRegistroCbr;

        public RegistrarFechaRegistroCbrController(
            IRegistrarFechaRegistroCbrApplication _registrarFechaRegistroCbrApplication,
            IBitacoraApplication _bitacoraApplication,
            IWorkflowApplication _workflowApplication)
        {
            RegistrarFechaRegistroCbrApplicationProvider = _registrarFechaRegistroCbrApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RegistrarFechaRegistroCbrApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Registrar Fecha Registro CBR obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] registrar_fecha_registro_cbr registrar_fecha_registro_cbr)
        {
            IActionResult response = Unauthorized();

            try
            {
                if (registrar_fecha_registro_cbr.id_registrar_fecha_registro_cbr == 0)
                {
                    registrar_fecha_registro_cbr.row_status = true;
                    registrar_fecha_registro_cbr.is_active = true;
                    registrar_fecha_registro_cbr = RegistrarFechaRegistroCbrApplicationProvider.Create(registrar_fecha_registro_cbr, GetUserId());
                }
                else
                {
                    registrar_fecha_registro_cbr = RegistrarFechaRegistroCbrApplicationProvider.Update(registrar_fecha_registro_cbr, GetUserId());
                }
                if (registrar_fecha_registro_cbr == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(registrar_fecha_registro_cbr.id_expediente, ActividadID);

                bitacora bitacora = new bitacora();
                bitacora.id_expediente = registrar_fecha_registro_cbr.id_expediente;
                bitacora.id_actividad = ActividadID;
                bitacora.id_usuario = GetUserId();
                bitacora.fecha_alta = DateTime.Now;
                bitacora.observaciones = registrar_fecha_registro_cbr.observaciones;
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
                    detail = registrar_fecha_registro_cbr,
                    message = "Registrar Fecha Registro CBR guardado correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await RegistrarFechaRegistroCbrApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

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
