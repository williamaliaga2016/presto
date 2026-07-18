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
    public class RectificatoriaFirmaController : ControllerBase
    {
        private readonly IRectificatoriaFirmaApplication RectificatoriaFirmaApplicationProvider;
        private readonly IRectificatoriaFirmaDetalleApplication RectificatoriaFirmaDetalleApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;
        private readonly ICommonApplication CommonApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.RectificatoriaFirma;

        public RectificatoriaFirmaController(
            IRectificatoriaFirmaApplication _rectificatoriaFirmaApplication,
            ICommonApplication commonApplicationProvider,
            IRectificatoriaFirmaDetalleApplication _rectificatoriaFirmaDetalleApplication,
            IBitacoraApplication _bitacoraApplication
        )
        {
            RectificatoriaFirmaApplicationProvider = _rectificatoriaFirmaApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
            CommonApplicationProvider = commonApplicationProvider;
            RectificatoriaFirmaDetalleApplicationProvider = _rectificatoriaFirmaDetalleApplication;
        }

        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                var result = await RectificatoriaFirmaApplicationProvider.GetByExpediente(
                    id_expediente
                );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Rectificatoria Firma obtenido correctamente."
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

        [HttpGet, Route("GetRectificatoriaDetByExpediente/{id_expediente}/{rol_comparecencia}")]
        public async Task<IActionResult> GetRectificatoriaDetByExpediente(long id_expediente, string rol_comparecencia)
        {
            try
            {
                var result = await RectificatoriaFirmaApplicationProvider.GetRectificatoriaDetByExpediente(
                    id_expediente,rol_comparecencia
                );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Rectificatoria Firma Detalle obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] rectificatoria_firma model)
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

                rectificatoria_firma modelSaved;

                if (model.id_rectificatoria_firma == 0)
                {
                    model.row_status = true;
                    model.is_active = true;

                    modelSaved = RectificatoriaFirmaApplicationProvider.Create(
                        model,
                        GetUserId()
                    );
                }
                else
                {
                    modelSaved = RectificatoriaFirmaApplicationProvider.Update(
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

                if (model.detalles != null && model.detalles.Any())
                {
                    foreach (var detalle in model.detalles)
                    {
                        detalle.id_rectificatoria_firma = modelSaved.id_rectificatoria_firma;
                        detalle.id_expediente = modelSaved.id_expediente;
                        detalle.row_status = true;
                        detalle.is_active = true;

                        if (detalle.id_rectificatoria_firma_detalle == 0)
                        {
                            RectificatoriaFirmaDetalleApplicationProvider.Create(
                                detalle,
                                GetUserId()
                            );
                        }
                        else
                        {
                            RectificatoriaFirmaDetalleApplicationProvider.Update(
                                detalle,
                                GetUserId()
                            );
                        }
                    }
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(
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

                rectificatoria_firma? result =
                    await RectificatoriaFirmaApplicationProvider.GetByExpediente(
                        modelSaved.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Rectificatoria Firma guardado correctamente."
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
                    await RectificatoriaFirmaApplicationProvider.Avanzar(
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

        [HttpGet, Route("GetControlesRectificatoriaFirma")]
        public async Task<IActionResult> GetControlesGenerarBorradorEscritura()
        {
            try
            {
                var _rolcomparecencia = await CommonApplicationProvider.GetCatalogoByType(Constants.Catalogo.RolComparecencia);


                return Ok(new
                {
                    status = true,
                    detail = new
                    {
                        rolcomparecencia = _rolcomparecencia

                    },
                    message = "Controles de RectificatoriaFirma obtenidos correctamente."
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
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
    }
}
