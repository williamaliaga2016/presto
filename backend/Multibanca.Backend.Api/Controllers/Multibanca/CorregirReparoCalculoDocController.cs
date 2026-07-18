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
    public class CorregirReparoCalculoDocController : ControllerBase
    {
        private readonly ICorregirReparoCalculoDocApplication CorregirReparoCalculoDocApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.CorregirReparoCalculoGenDoc;

        public CorregirReparoCalculoDocController(
            ICorregirReparoCalculoDocApplication _corregirReparoCalculoDocApplication,
            IBitacoraApplication _bitacoraApplication
        )
        {
            CorregirReparoCalculoDocApplicationProvider = _corregirReparoCalculoDocApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result =
                    await CorregirReparoCalculoDocApplicationProvider.GetByExpediente(
                        idExpediente
                    );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Cálculo y Generación Documento obtenido correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save(
            [FromBody] corregir_reparo_calculo_doc model
        )
        {
            IActionResult response = Unauthorized();

            try
            {
                if (model.id_usuario_solicitante == 0)
                {
                    model.id_usuario_solicitante = GetUserId();
                }

                if (model.is_subsanar && string.IsNullOrWhiteSpace(model.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones finales son obligatorias cuando se subsana el reparo."
                    });
                }

                if (string.Equals(model.existe_rol_avaluo, "SI", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(model.rol_avaluo_editado))
                    {
                        return BadRequest(new
                        {
                            status = false,
                            message = "El Rol Avalúo es obligatorio cuando ¿Existe Rol Avalúo? es Sí."
                        });
                    }

                    if (!model.valor_avaluo_pesos.HasValue)
                    {
                        return BadRequest(new
                        {
                            status = false,
                            message = "El Valor Avalúo Pesos es obligatorio cuando ¿Existe Rol Avalúo? es Sí."
                        });
                    }
                }
                else if (string.Equals(model.existe_rol_avaluo, "E/T", StringComparison.OrdinalIgnoreCase))
                {
                    model.rol_avaluo_editado = null;
                    model.valor_avaluo_pesos = null;
                }

                corregir_reparo_calculo_doc modelSaved;

                if (model.id_corregir_reparo_calculo_doc == 0)
                {
                    model.row_status = true;
                    model.is_active = true;

                    modelSaved = CorregirReparoCalculoDocApplicationProvider.Create(
                        model,
                        GetUserId()
                    );
                }
                else
                {
                    modelSaved = CorregirReparoCalculoDocApplicationProvider.Update(
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

                corregir_reparo_calculo_doc? result =
                    await CorregirReparoCalculoDocApplicationProvider.GetByExpediente(
                        modelSaved.id_expediente
                    );

                response = Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Corregir Reparo Cálculo y Generación Documento guardado correctamente."
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

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            IActionResult response = Unauthorized();

            try
            {
                int id_usuario = GetUserId();

                var current =
                    await CorregirReparoCalculoDocApplicationProvider.GetByExpediente(
                        id_expediente
                    );

                if (current == null || !current.is_subsanar)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "No se puede avanzar porque el reparo no se encuentra subsanado."
                    });
                }

                List<AssignActivityDTO> listAssignActividadesDTO =
                    await CorregirReparoCalculoDocApplicationProvider.Avanzar(
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
