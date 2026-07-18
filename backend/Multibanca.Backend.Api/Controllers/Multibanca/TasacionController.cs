using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasacionController : ControllerBase
    {
        private readonly ITasacionApplication TasacionApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.RegistrarTasacion;

        public TasacionController(
            ITasacionApplication _tasacionApplication,
            IBitacoraApplication _bitacoraApplication)
        {
            TasacionApplicationProvider = _tasacionApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                tasacion? result = await TasacionApplicationProvider.GetByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Tasación obtenida correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        //[Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("GetDetallesByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetDetallesByExpediente(long id_expediente)
        {
            try
            {
                List<tasacion_detalle> result = await TasacionApplicationProvider.GetDetallesByExpediente(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Tasaciones registradas obtenidas correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromBody] tasacion model)
        {
            try
            {
                int userId = GetUserId();

                if (model == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El modelo de Tasación es obligatorio."
                    });
                }

                if (model.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "El expediente es obligatorio."
                    });
                }

                if (model.is_enviar_reparo && string.IsNullOrWhiteSpace(model.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se envía a reparo."
                    });
                }

                tasacion saved = await TasacionApplicationProvider.Save(model, userId);

                if (saved == null)
                {
                    return BadRequest("Invalid client request");
                }

                var resultBitacora = await BitacoraApplicacionProvider.GetByExpedienteActividad(saved.id_expediente, ActividadID);

                bitacora bitacora = new bitacora
                {
                    id_expediente = saved.id_expediente,
                    id_actividad = ActividadID,
                    id_usuario = userId,
                    fecha_alta = DateTime.Now,
                    observaciones = saved.observaciones,
                    is_active = true,
                    row_status = true
                };

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicacionProvider.Update(bitacora, userId);
                }
                else
                {
                    BitacoraApplicacionProvider.Create(bitacora, userId);
                }

                return Ok(new
                {
                    status = true,
                    detail = saved,
                    message = "Tasación guardada correctamente."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpDelete, Route("EliminarDetalle/{id_tasacion_detalle}")]
        public async Task<IActionResult> EliminarDetalle(int id_tasacion_detalle)
        {
            try
            {
                int userId = GetUserId();
                bool result = await TasacionApplicationProvider.DeleteDetalle(id_tasacion_detalle, userId);

                return Ok(new
                {
                    status = result,
                    detail = result,
                    message = result ? "Tasación detalle eliminada correctamente." : "No se pudo eliminar la tasación detalle."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("EvaluarReparoAutomatico/{id_expediente}")]
        public async Task<IActionResult> EvaluarReparoAutomatico(long id_expediente)
        {
            try
            {
                EvaluarReparoAutomaticoDTO result = await TasacionApplicationProvider.EvaluarReparoAutomatico(id_expediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = result.aplica_reparo_automatico
                        ? "Aplica reparo automático."
                        : "No aplica reparo automático."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            try
            {
                int id_usuario = GetUserId();

                List<AssignActivityDTO> listAssignActividadesDTO = await TasacionApplicationProvider
                    .Avanzar(id_expediente, id_usuario, ActividadID);

                if (listAssignActividadesDTO.Count > 0)
                {
                    return Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente."
                    });
                }

                return Ok(new
                {
                    status = false,
                    detail = "",
                    message = "No se pudo avanzar la actividad."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = (object?)null,
                    message = ex.Message
                });
            }
        }

        private int GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity?.Claims == null)
            {
                return 0;
            }

            int index = 0;
            foreach (Claim claim in identity.Claims)
            {
                if (index == 2)
                {
                    return int.TryParse(claim.Value, out int userId) ? userId : 0;
                }

                index++;
            }

            return 0;
        }
    }
}
