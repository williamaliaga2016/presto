using Framework.WorkFlow.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargarDocumentosConstructoraController : ControllerBase
    {
        private readonly ICargarDocumentosConstructoraApplication CargarDocumentosConstructoraApplicationProvider;

        private readonly string ActividadID = Constants.ActividadesBBVA.DocsConstructora;

        public CargarDocumentosConstructoraController(
            ICargarDocumentosConstructoraApplication cargarDocumentosConstructoraApplication)
        {
            CargarDocumentosConstructoraApplicationProvider = cargarDocumentosConstructoraApplication;
        }

        [HttpGet, Route("GetByExpediente/{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await CargarDocumentosConstructoraApplicationProvider.GetByExpediente(idExpediente);

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Cargar Documentos Constructora obtenido correctamente."
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

        [HttpPost, Route("Guardar")]
        public IActionResult Guardar([FromBody] cargar_documentos_constructora payload)
        {
            try
            {
                if (payload.id == 0)
                {
                    payload.id_actividad = ActividadID;
                    payload.row_status = true;
                    payload.is_active = true;
                    payload = CargarDocumentosConstructoraApplicationProvider.Create(payload, GetUserId());
                }
                else
                {
                    payload = CargarDocumentosConstructoraApplicationProvider.Update(payload, GetUserId());
                }

                if (payload == null)
                    return BadRequest("Invalid client request");

                return Ok(new
                {
                    status = true,
                    detail = payload,
                    message = "Cargar Documentos Constructora guardado correctamente."
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

        [HttpPost, Route("Avanzar/{idExpediente}")]
        public async Task<IActionResult> Avanzar(long idExpediente)
        {
            try
            {
                List<AssignActivityDTO> result = await CargarDocumentosConstructoraApplicationProvider.Avanzar(
                    idExpediente,
                    GetUserId(),
                    ActividadID
                );

                return Ok(new
                {
                    status = true,
                    detail = result,
                    message = "Actividad avanzada correctamente."
                });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new
                {
                    status = false,
                    message = ex.Message
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
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            return int.Parse(claims[2].Value);
        }
        #endregion
    }
}
