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
    public class CorregirReparoTasacionController : ControllerBase
    {
        private readonly ICorregirReparoTasacionApplication CorregirReparoTasacionApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.CorregirReparoTasacion;

        public CorregirReparoTasacionController(
            ICorregirReparoTasacionApplication _corregirReparoTasacionApplication,
            IBitacoraApplication _bitacoraApplication
        )
        {
            CorregirReparoTasacionApplicationProvider = _corregirReparoTasacionApplication;
            BitacoraApplicationProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result =
                    await CorregirReparoTasacionApplicationProvider.GetByExpediente(
                        idExpediente
                    );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Tasación obtenido correctamente."
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
            [FromBody] corregir_reparo_tasacion model
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

                corregir_reparo_tasacion modelSaved;

                if (model.id_corregir_reparo_tasacion == 0)
                {
                    model.row_status = true;
                    model.is_active = true;

                    modelSaved = CorregirReparoTasacionApplicationProvider.Create(
                        model,
                        GetUserId()
                    );
                }
                else
                {
                    modelSaved = CorregirReparoTasacionApplicationProvider.Update(
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

                if (modelSaved.is_subsanar)
                {
                    await CorregirReparoTasacionApplicationProvider.MarcarReparoTasacionSubsanado(
                        modelSaved.id_expediente,
                        GetUserId()
                    );
                }

                var resultBitacora =
                    await BitacoraApplicationProvider.GetByExpedienteActividad(
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
                    BitacoraApplicationProvider.Update(bitacora, GetUserId());
                }
                else
                {
                    BitacoraApplicationProvider.Create(bitacora, GetUserId());
                }

                corregir_reparo_tasacion? result =
                    await CorregirReparoTasacionApplicationProvider.GetByExpediente(
                        modelSaved.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Tasación guardado correctamente."
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
            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await CorregirReparoTasacionApplicationProvider.Avanzar(
                        id_expediente,
                        id_usuario,
                        ActividadID
                    );

                return Ok(new
                {
                    status = listAssignActividadesDTO.Count > 0,
                    detail = listAssignActividadesDTO,
                    message = listAssignActividadesDTO.Count > 0
                        ? "Actividad avanzada correctamente."
                        : "No hay transiciones disponibles."
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

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity!.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
