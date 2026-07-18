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
    public class CalculoGeneracionDocumentoController : ControllerBase
    {
        private readonly ICalculoGeneracionDocumentoApplication CalculoApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicacionProvider;

        private readonly string ActividadID = Constants.Actividades.CalculoGeneracionDocumento;

        public CalculoGeneracionDocumentoController(
            ICalculoGeneracionDocumentoApplication _calculoApplication,
            IBitacoraApplication _bitacoraApplication)
        {
            CalculoApplicationProvider = _calculoApplication;
            BitacoraApplicacionProvider = _bitacoraApplication;
        }

        [Authorize(Roles = "ADMINISTRADOR,EJECUTIVO_FORMALIZADOR")]
        [HttpGet, Route("GetByExpediente/{id_expediente}")]
        public async Task<IActionResult> GetByExpediente(long id_expediente)
        {
            try
            {
                calculo_generacion_documento? result = await CalculoApplicationProvider.GetByExpediente(id_expediente);
                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Datos cargados correctamente."
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
        public async Task<IActionResult> Save([FromBody] calculo_generacion_documento model)
        {
            try
            {
                int userId = GetUserId();
                if (model.id_expediente == 0)
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
                calculo_generacion_documento saved;
                if (model.id_calculo_generacion_documento == 0)
                {
                    model.row_status = true;
                    model.is_active = true;
                    saved = CalculoApplicationProvider.Create(model, userId);
                }
                else
                {
                    saved = CalculoApplicationProvider.Update(model, userId);
                }
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
                    message = "Guardado correctamente."
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
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            try
            {
                int id_usuario = GetUserId();
                List<AssignActivityDTO> listAssignActividadesDTO = await CalculoApplicationProvider
                    .Avanzar(id_expediente, id_usuario, ActividadID);
                if (listAssignActividadesDTO.Count > 0)
                {
                    return Ok(new
                    {
                        status = true,
                        detail = "",
                        message = "Actividad avanzada correctamente"
                    });
                }
                return Ok(new
                {
                    status = false,
                    detail = "",
                    message = "No se pudo avanzar la actividad"
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
        [HttpGet, Route("CalcularUF")]
        public async Task<IActionResult> CalcularUF([FromQuery] DateTime fecha)
        {
            try
            {
                decimal? valor = await CalculoApplicationProvider.CalcularUF(fecha);
                if (!valor.HasValue)
                {
                    return Ok(new
                    {
                        status = false,
                        detail = 0m,
                        message = "No existe valor UF cargado para la fecha indicada. Contacte al administrador."
                    });
                }

                return Ok(new
                {
                    status = true,
                    detail = valor.Value,
                    message = "Valor UF calculado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = false,
                    detail = 0m,
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
