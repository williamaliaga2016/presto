using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.FuncTransversal;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.FuncTransversal;
using Multibanca.Domain.Models.Multibanca;
using Multibanca.DTO.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerarMemoEscrituraController : ControllerBase
    {
        private readonly IGenerarMemoEscrituraApplication ApplicationProvider;
        private readonly IBitacoraApplication BitacoraApplicationProvider;

        private readonly string ActividadID = Constants.Actividades.GenerarMemoEscritura;

        public GenerarMemoEscrituraController(
            IGenerarMemoEscrituraApplication application,
            IBitacoraApplication bitacoraApplication)
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
                    message = "Generar Memo Escritura obtenido correctamente."
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
        public async Task<IActionResult> Save([FromBody] generar_memo_escritura model)
        {
            try
            {
                if (model == null || model.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Debe enviar un id_expediente válido."
                    });
                }

                if (model.enviar_reparo && string.IsNullOrWhiteSpace(model.observaciones))
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Las observaciones son obligatorias cuando se envía a reparo."
                    });
                }

                int id_usuario = GetUserId();

                if (model.id_generar_memo_escritura == 0)
                {
                    model.row_status = true;
                    model.is_active = true;
                    model = ApplicationProvider.Create(model, id_usuario);
                }
                else
                {
                    model = ApplicationProvider.Update(model, id_usuario);
                }

                var resultBitacora = await BitacoraApplicationProvider.GetByExpedienteActividad(model.id_expediente, ActividadID);
                bitacora bitacora = new bitacora
                {
                    id_expediente = model.id_expediente,
                    id_actividad = ActividadID,
                    id_usuario = id_usuario,
                    fecha_alta = DateTime.Now,
                    observaciones = model.observaciones ?? string.Empty,
                    is_active = true,
                    row_status = true
                };

                if (resultBitacora.id_bitacora > 0)
                {
                    bitacora.id_bitacora = resultBitacora.id_bitacora;
                    BitacoraApplicationProvider.Update(bitacora, id_usuario);
                }
                else
                {
                    BitacoraApplicationProvider.Create(bitacora, id_usuario);
                }

                return Ok(new
                {
                    status = true,
                    detail = model,
                    message = "Generar Memo Escritura guardado correctamente."
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
        [HttpGet, Route("Avanzar/{id_expediente}")]
        public async Task<IActionResult> Avanzar(long id_expediente)
        {
            try
            {
                int id_usuario = GetUserId();
                List<AssignActivityDTO> result = await ApplicationProvider.Avanzar(id_expediente, id_usuario, ActividadID);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Actividad avanzada correctamente."
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

        [HttpGet, Route("GetDatosMemo/{id_expediente}")]
        public async Task<IActionResult> GetDatosMemo(long id_expediente)
        {
            try
            {
                MemoEscrituraDataDTO data = await ApplicationProvider.GetDatosMemo(id_expediente);
                return Ok(new
                {
                    status = true,
                    detail = data,
                    message = "Datos del memo cargados correctamente."
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

        [HttpGet, Route("ListarVersiones/{id_expediente}")]
        public async Task<IActionResult> ListarVersiones(long id_expediente)
        {
            try
            {
                List<expediente_digital> versiones = await ApplicationProvider.ListarVersiones(id_expediente);
                return Ok(new
                {
                    status = true,
                    detail = versiones,
                    message = "Versiones del memo obtenidas correctamente."
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
        [HttpPost, Route("GenerarPdf")]
        public async Task<IActionResult> GenerarPdf([FromBody] MemoEscrituraDTO request)
        {
            try
            {
                if (request == null || request.id_expediente <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Debe enviar un id_expediente válido."
                    });
                }

                int id_usuario = GetUserId();
                expediente_digital memoIndexado = await ApplicationProvider.GenerarPdf(request, id_usuario);

                if (memoIndexado == null)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "No se pudo generar el memo de escritura."
                    });
                }

                return Ok(new
                {
                    status = true,
                    detail = memoIndexado,
                    message = "Memo de escritura generado e indexado correctamente."
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
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return 0;
                IList<Claim> claims = identity.Claims.ToList();
                if (claims.Count > 2 && int.TryParse(claims[2].Value, out int userId)) return userId;
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}
