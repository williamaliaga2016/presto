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
    public class RectificatoriaPostventaSolucionReparoController : ControllerBase
    {
        private readonly IRectificatoriaPostventaSolucionReparoApplication ApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.RectificatoriaPostventaSolucionReparo;

        public RectificatoriaPostventaSolucionReparoController(
            IRectificatoriaPostventaSolucionReparoApplication application,
            IBitacoraApplication bitacoraApplication
        )
        {
            ApplicationProvider = application;
            BitacoraApplicationProvider = bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                var result = await ApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Generar Memo Escritura obtenido correctamente."
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
            [FromBody] rectificatoria_postventa_solucion_reparo model
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

                rectificatoria_postventa_solucion_reparo modelSaved;

                if (model.id_rectificatoria_postventa_solucion_reparo == 0)
                {
                    model.row_status = true;
                    model.is_active = true;

                    modelSaved = ApplicationProvider.Create(model, GetUserId());
                }
                else
                {
                    modelSaved = ApplicationProvider.Update(model, GetUserId());
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

                rectificatoria_postventa_solucion_reparo? result =
                    await ApplicationProvider.GetByExpediente(
                        modelSaved.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Generar Memo Escritura guardado correctamente."
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
                    await ApplicationProvider.Avanzar(
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

        private int GetUserId()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated)
                {
                    return 0;
                }

                IList<Claim> claims = identity.Claims.ToList();

                if (claims.Count > 2 && int.TryParse(claims[2].Value, out int userId))
                {
                    return userId;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
