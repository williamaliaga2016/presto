using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorregirReparoInstPagoController : ControllerBase
    {
        private readonly ICorregirReparoInstPagoApplication CorregirReparoInstPagoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = "corregir_reparo_inst_pago";

        public CorregirReparoInstPagoController(
            ICorregirReparoInstPagoApplication _corregirReparoInstPagoApplication,
            IBitacoraApplication _bitacoraApplication
        )
        {
            CorregirReparoInstPagoApplicationProvider = _corregirReparoInstPagoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                var result =
                    await CorregirReparoInstPagoApplicationProvider.GetByExpediente(
                        id_expediente
                    );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Inst Pago obtenido correctamente."
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
        public async Task<IActionResult> Save(
            [FromBody] corregir_reparo_inst_pago model
        )
        {
            IActionResult response = Unauthorized();

            try
            {
                if (model.id_usuario_solicitante == 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El usuario solicitante del reparo es obligatorio."
                    });
                }

                if (model.is_subsanar && string.IsNullOrWhiteSpace(model.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se subsana el reparo."
                    });
                }

                corregir_reparo_inst_pago modelSaved;

                if (model.id_corregir_reparo_inst_pago == 0)
                {
                    model.row_status = true;
                    model.is_active = true;

                    modelSaved = CorregirReparoInstPagoApplicationProvider.Create(
                        model,
                        GetUserId()
                    );
                }
                else
                {
                    modelSaved = CorregirReparoInstPagoApplicationProvider.Update(
                        model,
                        GetUserId()
                    );
                }

                if (modelSaved == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Invalid client request"
                    });
                }

                var resultBitacora =
                    await BitacoraApplicacionProvider.GetByExpedienteActividad(
                        modelSaved.id_expediente,
                        ActividadID
                    );

                bitacora bitacora = new bitacora
                {
                    id_expediente = modelSaved.id_expediente,
                    id_actividad = ActividadID,
                    id_usuario = GetUserId(),
                    fecha_alta = DateTime.Now,
                    observaciones = modelSaved.observaciones ?? string.Empty,
                    is_active = true,
                    row_status = true
                };

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, GetUserId());
                }
                else
                {
                    BitacoraApplicacionProvider.Create(bitacora, GetUserId());
                }

                corregir_reparo_inst_pago? result =
                    await CorregirReparoInstPagoApplicationProvider.GetByExpediente(
                        modelSaved.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Inst Pago guardado correctamente."
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

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await CorregirReparoInstPagoApplicationProvider.Avanzar(
                        id_expediente,
                        id_usuario,
                        ActividadID
                    );

                if (listAssignActividadesDTO.Count > 0)
                {
                    response = Ok(new
                    {
                        status = true,
                        detail = listAssignActividadesDTO,
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

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity!.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
