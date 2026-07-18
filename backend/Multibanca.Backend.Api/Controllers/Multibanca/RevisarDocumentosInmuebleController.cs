using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Exceptions;
using Multibanca.Domain.Models.Multibanca;
using System.Security.Claims;

namespace Multibanca.Backend.Api.Controllers.Multibanca
{
    [Route("api/revisar-documentos-inmueble")]
    [ApiController]
    public class RevisarDocumentosInmuebleController : ControllerBase
    {
        private readonly IRevisarDocumentosInmuebleApplication RevisarDocumentosInmuebleProvider;
        private readonly string ActividadID = Constants.ActividadesBBVA.RevisarDocs;

        public RevisarDocumentosInmuebleController(IRevisarDocumentosInmuebleApplication revisarDocumentosInmuebleApplication)
        {
            RevisarDocumentosInmuebleProvider = revisarDocumentosInmuebleApplication;
        }

        [HttpGet, Route("{idExpediente}")]
        public async Task<IActionResult> GetByExpediente(long idExpediente)
        {
            try
            {
                var result = await RevisarDocumentosInmuebleProvider.GetByExpediente(idExpediente);
                return Ok(new { status = true, detail = result, message = "Datos consultados correctamente." });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpGet, Route("controles")]
        public async Task<IActionResult> GetControles()
        {
            try
            {
                var result = await RevisarDocumentosInmuebleProvider.GetControles();
                return Ok(new { status = true, detail = result, message = "Controles obtenidos correctamente." });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrió un error interno en el sistema." });
            }
        }

        [HttpPost, Route("guardar")]
        public async Task<IActionResult> Guardar([FromBody] revisar_documentos_inmueble request)
        {
            try
            {
                int userId = GetUserId();
                request.id_actividad = ActividadID;

                var result = request.id == 0
                    ? RevisarDocumentosInmuebleProvider.Create(request, userId)
                    : RevisarDocumentosInmuebleProvider.Update(request, userId);

                return Ok(new { status = true, detail = result, message = "Guardado con éxito." });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
            }
        }

        [HttpPost, Route("{idExpediente}/avanzar")]
        public async Task<IActionResult> Avanzar(long idExpediente)
        {
            try
            {
                var result = await RevisarDocumentosInmuebleProvider.Avanzar(idExpediente, GetUserId(), ActividadID);
                return Ok(new { status = true, detail = result, message = "Actividad avanzada correctamente." });
            }
            catch (BusinessException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = ex.Message });
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
